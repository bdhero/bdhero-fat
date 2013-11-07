using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    public class JobObjectFactory : IJobObjectFactory
    {
        public IJobObject CreateJobObject()
        {
            return new JobObject();
        }
    }
}
