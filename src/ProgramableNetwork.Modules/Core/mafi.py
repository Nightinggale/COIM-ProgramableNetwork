
class Fix32:
    """Represents type Mafi.Fix32"""
    def __init__(self, value: int) -> None:
        self.RawValue = value

    def FromRaw(value):
        return Fix32(value)
        

def fix(value) -> Fix32:
    """Built in function to convert any number value to Mafi.Fix32"""
    pass