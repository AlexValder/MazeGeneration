extends OptionButton
class_name AlgoOptionButton

onready var _mask := $"../MaskCheckBox" as CheckBox
var _info := {}


func initialize(names: Array) -> void:
    print("IN HERE!")
    for algo in names:
        print(algo)
        match typeof(algo):
            TYPE_STRING:
                self.add_item(algo)
                self._info[algo] = false
            TYPE_STRING_ARRAY:
                if algo.size() < 1:
                    push_warning("Got empty array.")
                elif algo.size() < 2:
                    self.add_item(algo[0])
                    self._info[algo[0]] = false
                else:
                    self.add_item(algo[0])
                    print("algo[1] = %s" % algo[1])
                    self._info[algo[0]] = (algo[1] == 'true')
            _:
                push_warning("Unknown type: %s" % typeof(algo))



func _on_AlgoOptionButton_item_selected(index):
    var name := self.get_item_text(index)
    print(self._info[name])
    _mask.visible = self._info[name]
