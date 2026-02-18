using System;
using System.Runtime.Serialization;

namespace CampusHire
{
    [Serializable]
    public class Applicant
    {
        public string ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string CurrentLocation { get; set; }
        public string PreferredLocation { get; set; }
        public string CoreCompetency { get; set; }
        public int PassingYear { get; set; }
    }
}
