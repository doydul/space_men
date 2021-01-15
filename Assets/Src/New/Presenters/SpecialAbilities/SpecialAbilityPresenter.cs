using System.Collections;

public abstract class SpecialAbilityPresenter<T> : Presenter {
  
    public abstract IEnumerator Present(T input);
}

