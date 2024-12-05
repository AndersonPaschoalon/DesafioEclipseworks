using System.Collections.ObjectModel;

namespace EclipseTaskManager.Models;

public class Report
{
    public Report()
    {
        userReports = new Dictionary<int, ReportUser>();
    }

    public Dictionary<int, ReportUser> userReports { get; set; }

    public List<int> allCompletedTasks
    {
        get 
        {
            List<int> ids = new List<int>();
            foreach(var entry in userReports)
            {
                ids.AddRange(entry.Value.tasksInDone.ToList());
            }
            return ids;
        }
    }
    public int averageDoneByUser
    {
        get
        {
            if (userReports.Count != 0)
            {
                return (int)allCompletedTasks.Count / userReports.Count;
            }
            return 0;
        }
    }

}
