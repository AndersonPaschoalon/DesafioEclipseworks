using System.Collections.ObjectModel;

namespace EclipseTaskManager.Models
{
    public class Report
    {

        public Report()
        {
            usersReports = new Collection<ReportUser>();
        }

        public ICollection<ReportUser> usersReports { get; set; }


        public ReportUser allUsersReport 
        {
            get
            {
                ReportUser allUsers = new ReportUser();
                foreach (ReportUser user in usersReports)
                {
                    // allUsers.tasksInProgress.AddRange(user.tasksInProgress);
                    // allUsers.tasksInToDo.AddRange(user.tasksInToDo);
                    allUsers.tasksInDone.AddRange(user.tasksInDone);
                }
                return allUsers;
            }
        }

        /*
        public int averageInProgress
        {
            get 
            {
                if (usersReports.Count != 0) 
                {
                    return (int) allUsersReport.tasksInProgress.Count / usersReports.Count;
                }
                return 0;
            }
        }
        */

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

        /*
        public int averageTodo
        {
            get
            {
                if (usersReports.Count != 0)
                {
                    return (int)allUsersReport.tasksInToDo.Count / usersReports.Count;
                }
                return 0;
            }
        }
        */


    }
}
