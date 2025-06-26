namespace FSM;
internal interface IActivatable
{
    bool IsActive { get; set; }
    bool ResetOnActivation {  get; set; }
    void Activate();
    void Deactivate();
    void Reset();
}
