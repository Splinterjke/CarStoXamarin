namespace CarSto.Presenters.Garage
{
    public interface IGaragePresenter
    {
        IGarageView View { get; set; }
        void LoadGarage();
        void OnLoadSuccessful(string response);
        void OnLoadFailed(string error);
        void OnCarRemoved(int index);
        void OnMileageEdited();
    }
}