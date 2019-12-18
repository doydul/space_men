using Interactors;
using Data;

// { typeof(WorkshopController),
//     new Dictionary<Type, Type> {
//         { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
//     }
// }
public class WorkshopController : Controller {

    // public DoSomeActionInteractor doSomeActionInteractor { get; set; }

    public void DoSomeAction() {
        if (!disabled) {
            // doSomeActionInteractor.Interact(new DoSomeActionInput());
        }
    }
}
