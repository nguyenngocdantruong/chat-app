namespace ChatApp.Domain.Enums
{
    public enum ConverstationEventType
    {
        #region Conversation Events
        Created,
        Renamed,
        AvatarUpdated,
        ThemeUpdated,
        EmojiUpdated,
        #endregion

        #region Member Events
        MemberAdded,
        MemberRemoved,
        MemberRoleUpdated,
        MemberNicknameUpdated,
        #endregion

        #region Message Events
        PinnedMessage,
        ReactedToMessage,
        #endregion

    }
}
