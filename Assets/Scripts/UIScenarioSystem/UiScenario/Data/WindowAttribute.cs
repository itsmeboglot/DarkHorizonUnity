namespace Core.UiScenario.Data
{
    public enum WindowAttribute
    {
        None = 1,
        //Elements of the graphical interface - requiring a specific user action, which does not allow switching to other similar objects
        Modal = 1 << 1,
        IgnoreSort = 1 << 2,
        //It is the goal of adaptation for devices like IPhoneX
        IphoneXAdapt = 1 << 3,
        InternetDependable = 1 << 4,
        //Whether multiple window openings are allowed.
        AsTransient = 1 << 5
    }
}