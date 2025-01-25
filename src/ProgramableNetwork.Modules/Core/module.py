from Core.categories import Category
from Core.fields import Field, FieldValue
from Core.entities import Entity
from Core.io import Input, Output, InputValue, OutputValue

class DefaultControllers:
    Controller = "ProgramableNetwork_Controller"

class ModuleStatusValue:
    pass

class ModuleStatus:
    Init = ModuleStatusValue()
    Running = ModuleStatusValue()
    Error = ModuleStatusValue()
    Paused = ModuleStatusValue()

class Module:
    name: str = "module display name"
    inputs: list[Input] = []
    outputs: list[Output] = []
    fields: list[Field] = []
    categories: list[Category] = []
    controllers: list[str] = []

    def __init__(self):
        # defines an interface to data of input inside module
        self.Input = InputValue(self)
        # defines an interface to data of output inside module
        self.Output = OutputValue(self)
        # defines an interface to data of field inside module
        self.Field = FieldValue(self)
        # defines an interface to raw data inside module
        self.NumberData = {}
        # defines an interface to raw data inside module
        self.StringData = {}
        # defines status of module
        self.Status = ModuleStatus.Init
        # defines an Tooltip/Error info
        self.Error = ""

    def init(self, prototype):
        pass

    def action(self):
        pass
