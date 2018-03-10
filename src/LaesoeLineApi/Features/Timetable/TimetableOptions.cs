using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Features.Timetable
{
    public class TimetableOptions : IOptions<TimetableOptions>
    {
        public int InitialCrawlDays { get; set; }
        public int HourlyCrawlDays { get; set; }
        public int DailyCrawlDays { get; set; }

        TimetableOptions IOptions<TimetableOptions>.Value => this;
    }
}
