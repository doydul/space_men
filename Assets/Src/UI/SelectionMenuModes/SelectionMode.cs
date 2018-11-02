using System.Collections.Generic;

public interface SelectionMode {

    List<SelectableItem> selectableItems { get; }

    void Select(SelectableItem item);

    void Finalise();
}
