using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace EducationManagementSystem.Jobs;

public class CreateMarksViaGpsJob(AppDbContext dbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        if (context.MergedJobDataMap["gpsSessionId"] is not Guid sessionId)
            throw new Exception();
        var session = await dbContext.MarkSessions
            .OfType<GpsMarkSession>()
            .Include(x => x.Points)
            .Include(x => x.Source)
            .Where(x => x.Id == sessionId)
            .SingleAsync(context.CancellationToken);
        var relativeDistances = session.Points.Select(point =>
                (
                    Distance: 6371d * Math.Acos(Math.Sin(session.Source.Latitude) * Math.Sin(point.Latitude) +
                                                Math.Cos(session.Source.Longitude) * Math.Cos(point.Longitude) *
                                                Math.Cos(session.Source.Longitude - point.Longitude)),
                    Subpoint: point)
            )
            .ToList();
        var points = relativeDistances.Select(x => new Point()
        {
            X = x.Distance,
            RelativePoint = x.Subpoint
        }).ToList();
        Cluster(points);
        var toMark = points.Where(x => x.SelectedCluster == 0);
        foreach (var mark in toMark.Select(x => x.RelativePoint))
        {
            await dbContext.Marks.AddAsync(new Mark()
            {
                StudentId = mark.StudentId ?? throw new Exception(),
                LessonId = session.LessonId,
                Passed = true
            });
        }
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
    private class Point
    {
        public double X { get; init; }
        public GpsPoint RelativePoint { get; init; } = default!;
        public int SelectedCluster { get; set; }
    }
    private const int Clusters = 2;
    private static void Cluster(List<Point> dataPoints)
        {
            var centroids = InitializeRandomCentroids(dataPoints);
            for (var iteration = 0; iteration < 1000; iteration++)
            {
                // Assign data points to the nearest cluster
                foreach (var dataPoint in dataPoints)
                {
                    var minDistance = double.MaxValue;
                    var closestCluster = -1;
                    for (var i = 0; i < Clusters; i++)
                    {
                        var distance = CalculateDistance(dataPoint, centroids[i]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestCluster = i;
                        }
                    }
                    dataPoint.SelectedCluster = closestCluster;
                }
                // Update centroids
                for (var i = 0; i < Clusters; i++)
                {
                    var clusterPoints = dataPoints
                        .Where(p => p.SelectedCluster == i)
                        .ToList();
                    if (clusterPoints.Count <= 0)
                        continue;
                    var meanX = clusterPoints
                            .Select(p => p.X)
                            .Average();
                    centroids[i] = new() { X = meanX };
                }
            }
        }
        private static List<Point> InitializeRandomCentroids(List<Point> dataPoints)
        {
            var centroids = new List<Point>();
            for (var i = 0; i < Clusters; i++)
            {
                var randomIndex = Random.Shared.Next(dataPoints.Count - 1);
                centroids.Add(new()
                {
                    X = dataPoints[randomIndex].X,
                    SelectedCluster = i
                });
            }
            return centroids;
        }
        private static double CalculateDistance(Point a, Point b)
        {
            var dx = a.X - b.X;
            return Math.Sqrt(dx * dx);
        }
}