using System.Collections.ObjectModel;

namespace EclipseTaskManager.Models;

public class Report
{

    public Report()
    {
        // usersReports = new List<ReportUser>();
        
    }

    public Dictionary<int, ReportUser> userReports;

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

    // public List<ReportUser> usersReports { get; set; }

    /*
    public ReportUser allUsersReport 
    {
        get
        {
            ReportUser allUsers = new ReportUser { tasksInDone = new List<int>() };
            foreach (ReportUser user in usersReports)
            {
                allUsers.tasksInDone.AddRange(user.tasksInDone);
            }
            return allUsers;
        }
    }


    public int averageDone
    {
        get
        {
            if (usersReports.Count != 0)
            {
                return (int)allUsersReport.tasksInDone.Count / usersReports.Count;
            }
            return 0;
        }
    }
    */
}
