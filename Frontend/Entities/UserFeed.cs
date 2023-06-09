﻿using Frontend.Service;

namespace Frontend.Entities;

public class UserFeed
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public int NumberOfReactions { get; set; }
    public FeedPostDtoTopic Topic { get; set; }
    public DateTimeOffset TimeOfActivity { get; set; }
    public ActivityData? ActivityData { get; set; }
    public List<Comment>? Comments { get; set; }
}