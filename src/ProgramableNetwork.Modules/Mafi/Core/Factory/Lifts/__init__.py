class Lift:

    def __init__(self):
        self.Prototype = None
        self.CanBePaused = False
        self.AnimationParams = None
        self.AnimationStatesProvider = None
        self.IsGeneralPriorityVisible = False
        from Mafi import Option
        self.ElectricityConsumer = Option()
        self.MaxBufferSize = None
        self.QuantityInOutputBuffer = None
        from Mafi import Option
        self.CustomTitle = Option()
        self.GeneralPriority = int(0)
        self.IsCargoAffectedByGeneralPriority = False
        self.Ports = None
        self.Value = None
        self.ConstructionCost = None
        self.Transform = None
        self.OccupiedTiles = None
        self.OccupiedVertices = None
        self.OccupiedVerticesCombinedConstraint = None
        self.VehicleSurfaceHeights = None
        self.PfTargetTiles = None
        self.CenterTile = None
        self.Position2f = None
        self.Position3f = None
        self.ConstructionState = None
        self.IsConstructed = False
        self.IsNotConstructed = False
        self.IsBeingUpgraded = False
        from Mafi import Option
        self.ConstructionProgress = Option()
        self.DoNotAdjustTerrainDuringConstruction = False
        self.AreConstructionCubesDisabled = False
        self.Id = None
        self.DefaultTitle = None
        self.Context = None
        self.IsDestroyed = False
        self.IsEnabled = False
        self.IsNotEnabled = False
        self.IsPaused = False
        self.IsNotPaused = False
        self.RendererData = None
        self.PowerRequired = None
class ILiftProto:

    def __init__(self):
        self.HeightDelta = None
        self.MinHeightDelta = None
        self.PortsShape = None
        self.Layout = None
        self.Ports = None
        self.CannotBeReflected = False
        self.IsUnique = False
        self.AutoBuildMiniZippers = False
        self.Graphics = None
        self.Id = None
        self.EntityType = None
        self.Costs = None
        self.Strings = None
        self.IsAvailable = False
        self.IsNotAvailable = False
        self.Mod = None
class LiftProto:

    def __init__(self):
        self.EntityType = None
        self.CanBeElevated = False
        self.CanPillarsPassThrough = False
        self.ElectricityConsumed = None
        self.PortsShape = None
        self.AnimationParams = None
        self.HeightDelta = None
        self.MinHeightDelta = None
        self.MaxThroughputPerStep = None
        self.AutoBuildMiniZippers = False
        self.Graphics = None
        self.Layout = None
        self.Ports = None
        self.CloningDisabled = False
        self.IsUnique = False
        self.CannotBeReflected = False
        self.IconPath = str(0)
        self.Id = None
        self.Costs = None
        self.Strings = None
        self.IsNotPhantom = False
        self.Mod = None
        self.Tags = None
        self.IsNotAvailable = False
        self.IsAvailable = False
        self.IsObsolete = False
class Gfx:

    def __init__(self):
        self.PrefabPath = str(0)
        self.PrefabOrigin = None
        self.IconPath = str(0)
        self.VisualizedLayers = None
        self.Categories = None
class ReverseLiftCmd:

    def __init__(self):
        self.AffectsSaveState = False
        self.IsProcessed = False
        self.IsProcessedAndSynced = False
        self.ProcessedAtStep = None
        self.ResultSet = False
        self.IsVerificationCmd = False
        self.Result = False
        self.HasError = False
        self.ErrorMessage = str(0)
