using System.Collections.ObjectModel;

namespace EclipseTaskManager.Models
{
    public class ReportUser
    {
        public ReportUser()
        {
            userId = 0;
            // tasksInProgress = new List<int>();
            // tasksInToDo = new List<int>();
            tasksInDone = new List<int>();
        }

        public int userId;
        //public List<int> tasksInProgress;
        //public List<int> tasksInToDo;
        public List<int> tasksInDone;

    }
}
