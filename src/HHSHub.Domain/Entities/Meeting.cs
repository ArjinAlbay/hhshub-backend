using HHSHub.Domain.Common;

namespace HHSHub.Domain.Entities;

public class Meeting : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private  set; } 
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int Duration { get; private set; } 
    public string Location { get; private set; } 
    public Guid OrganizerId { get; private set; }
    // public User? Organizer { get; private set; }
    public List<Guid> ParticipantIds  { get; private  set; } = new();
    
    private Meeting(string title,
        string description,
       DateTime startTime,
        DateTime endTime,
        int duration,
        Guid organizerId)
    {
       Title = title;
       Description = description ?? string.Empty;
       StartTime = startTime;
       EndTime = endTime;
       Duration = duration;
       OrganizerId = organizerId;
      
    }

    public static Meeting Create(string title,
        string description,
        DateTime scheduledAt,
        int duration,
        Guid organizerId)
    {
        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        if(duration <= 0)
            throw new ArgumentException("Duration must be greater than zero.", nameof(duration));
        if(organizerId == Guid.Empty)
            throw new ArgumentException("OrganizerId cannot be empty.", nameof(organizerId));
        if(scheduledAt < DateTime.UtcNow)
            throw new ArgumentException("ScheduledAt cannot be in the past.", nameof(scheduledAt));
        
        return new Meeting(
            title: title,
            description: description ?? string.Empty,
            startTime: scheduledAt,
            endTime: scheduledAt.AddMinutes(duration),
            duration: duration,
            organizerId: organizerId    
            
          
        );
    }
    
    public void UpdateDetails(string title,
        string description,
        DateTime scheduledAt,
        int duration)
    {
        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        if(duration <= 0)
            throw new ArgumentException("Duration must be greater than zero.", nameof(duration));
        if(scheduledAt < DateTime.UtcNow)
            throw new ArgumentException("ScheduledAt cannot be in the past.", nameof(scheduledAt));
        
        Title = title;
        Description = description ?? string.Empty;
        StartTime = scheduledAt;
        EndTime = scheduledAt.AddMinutes(duration);
        Duration = duration;
        

        SetUpdated();
    }
    
    public void Reschedule(DateTime newStartTime, int duration)
    {
        if(newStartTime < DateTime.UtcNow)
            throw new ArgumentException("New start time cannot be in the past.", nameof(newStartTime));
        
        if(duration <= 0)
            throw new ArgumentException("Duration must be greater than zero.", nameof(duration));
        StartTime = newStartTime;
        EndTime = newStartTime.AddMinutes(duration);
        Duration = duration;
       
        
        SetUpdated();
    }
    
    public void AddParticipant(Guid participantId)
    {
        if(participantId == Guid.Empty)
            throw new ArgumentException("ParticipantId cannot be empty.", nameof(participantId));
        
        if(!ParticipantIds.Contains(participantId))
        {
            ParticipantIds.Add(participantId);
            SetUpdated();
        }
    }
}