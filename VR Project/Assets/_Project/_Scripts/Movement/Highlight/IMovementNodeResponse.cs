public interface IMovementNodeResponse
{
    public void Selected(bool allow);

    public void Deselected();

    public void SetActive(bool active);
}