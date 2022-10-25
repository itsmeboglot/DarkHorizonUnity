using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Darkhorizon.Shared.Player.Dto;
using UseCases.Menu;
using Views.UI;

namespace Presenters.UI
{
    public class DeckManagerPresenter : Presenter
    {
        private class Controller : Controller<DeckManagerView>, DeckClick.ISubscribed
        {
            private readonly ProfileUseCase _profileUseCase;

            private DeckDto[] _decks;
            
            public Controller(DeckManagerView view, IWindowHandler windowHandler, ProfileUseCase profileUseCase) : base(view, windowHandler)
            {
                _profileUseCase = profileUseCase;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                var profile = _profileUseCase.GetProfile();
                
                _decks = profile.Decks;
            }

            public void OnEvent(int deckId)
            {
                
            }

            private void CheckDecksCapacity()
            {
                if (_decks == null || _decks.Length == 0)
                {
                    
                }
            }
        }
        protected override void InstallBindings()
        {
            
        }

        public class DeckClick : EventHub<DeckClick, int>
        {
        }
    }
}