using AIMarbles.Core;
using AIMarbles.Core.Interface;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AIMarbles.ViewModel
{
    public partial class PalleteViewModel: ViewModelBase
    {
        private readonly ICanvasObjectService _canvasObjectService;
        public PalleteViewModel(ICanvasObjectService canvasObjectService)
        {
            _canvasObjectService = canvasObjectService;
        }

        [RelayCommand]
        private void AddChannel()
        {
            Trace.WriteLine("Adding Channel");
            _canvasObjectService.AddChannel();
        }

        [RelayCommand]
        private void AddNote()
        {
            Trace.WriteLine("Adding Note");
            _canvasObjectService.AddNote();
        }

        [RelayCommand]
        private void AddMetronom()
        {
            Trace.WriteLine("Adding Metronom");
            _canvasObjectService.AddMetronom();
        }

        [RelayCommand]
        private void AddOperator()
        {
            Trace.WriteLine("Adding Operator");
            _canvasObjectService.AddOperator();
        }

        [RelayCommand]
        private void RemoveItem()
        {
            _canvasObjectService.RemoveSelectedCanvasObjects();
        }

    }
}
