using System;
using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class GameWizardViewModel : ValidatableViewModelBase
    {
        public GameFeaturesViewModel GameFeaturesViewModel { get; }
        public GameWizardBasicViewModel GameWizardBasicViewModel { get; }
        public RoomsViewModel RoomsViewModel { get; }
        public PlaceableObjectsViewModel PlaceableObjectsViewModel { get; }
        public ContainersViewModel ContainersViewModel { get; }
        public NpcsViewModel NpcsViewModel { get; }

        public GameWizardViewModel()
        {
            GameFeaturesViewModel = new GameFeaturesViewModel();
            GameWizardBasicViewModel = new GameWizardBasicViewModel();
            RoomsViewModel = new RoomsViewModel();
            PlaceableObjectsViewModel = new PlaceableObjectsViewModel();
            ContainersViewModel = new ContainersViewModel();
            NpcsViewModel = new NpcsViewModel();

            SetupSubcriptions();
        }

        private void SetupSubcriptions()
        {
            Disposables.Add(this.WhenAnyValue(x => x.GameFeaturesViewModel.EnablePlayerLost).Subscribe(
                _ => GameWizardBasicViewModel.EnablePlayerLost = GameFeaturesViewModel.EnablePlayerLost));
            
            Disposables.Add(this.WhenAnyValue(x => x.GameFeaturesViewModel.EnableTitles).Subscribe(
                _ => GameWizardBasicViewModel.EnableTitles = GameFeaturesViewModel.EnableTitles));

            Disposables.Add(this.WhenAnyValue(x => x.GameFeaturesViewModel.EnableScore).Subscribe(
                _ => GameWizardBasicViewModel.EnableScore = GameFeaturesViewModel.EnableScore));
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                GameFeaturesViewModel.Dispose();
                GameWizardBasicViewModel.Dispose();
                RoomsViewModel.Dispose();
                PlaceableObjectsViewModel.Dispose();
                ContainersViewModel.Dispose();
                NpcsViewModel.Dispose();
            }
        }
    }
}