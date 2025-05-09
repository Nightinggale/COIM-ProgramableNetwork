class TreePlanter:

    def __init__(self):
        self.CargoPickupDuration = None
        self.ProductProto = None
        self.Cargo = None
        self.IsEmpty = False
        self.IsNotEmpty = False
        self.IsFull = False
        self.IsNotFull = False
        self.RemainingCapacity = None
        self.Capacity = None
        self.State = None
        self.StateChangedOnSimStep = None
        from Mafi import Option
        self.ForestryTower = Option()
        self.CurrentStateDuration = None
        self.CurrentStateRemaining = None
        self.ArmStateChangeSpeedFactor = None
        self.CabinDirectionRelative = None
        self.CanBePaused = False
        from Mafi import Option
        self.CustomTitle = Option()
        from Mafi import Option
        self.AssignedTo = Option()
        self.NeedsJob = False
        self.NeedsRefueling = False
        self.IsFuelTankEmpty = False
        self.CannotWorkDueToLowFuel = False
        self.CanRunWithNoFuel = False
        from Mafi import Option
        self.FuelTank = Option()
        self.JobsCount = int(0)
        self.IsEngineOn = False
        self.IsOnWayToDepotForScrap = False
        self.IsOnWayToDepotForReplacement = False
        from Mafi import Option
        self.ReplacementProto = Option()
        self.ReplaceQueued = False
        self.CanBeAssigned = False
        self.HasJobs = False
        self.HasTrueJob = False
        from Mafi import Option
        self.CurrentJob = Option()
        self.IsIdle = False
        self.CurrentJobInfo = None
        self.IsStuck = False
        self.Maintenance = None
        self.GeneralPriority = int(0)
        self.IsCargoAffectedByGeneralPriority = False
        self.IsGeneralPriorityVisible = False
        self.IsNavigating = False
        self.NavigatedSuccessfully = False
        self.NavigationFailed = False
        self.NavigationFailedStreak = int(0)
        self.PfState = None
        self.TrackExploredTiles = False
        self.PfTask = None
        self.PathFindingResult = None
        from Mafi import Option
        self.NavigationGoal = Option()
        self.PathFindingParams = None
        from Mafi import Option
        self.UnreachableGoal = Option()
        self.IsStrugglingToNavigate = False
        from Mafi import Option
        self.CurrentPathSegment = Option()
        self.DrivingData = None
        self.Target = None
        self.CurrentOrLastDrivingTarget = None
        self.IsDriving = False
        self.IsMoving = False
        self.Speed = None
        self.SpeedPercentOfPeak = None
        self.AccelerationPercentOfPeak = None
        self.SteeringAngle = None
        self.SteeringAccelerationPercent = None
        self.DistanceToFullStop = None
        self.TargetIsTerminal = False
        self.DrivingState = None
        self.SpeedFactor = None
        self.CurrentRoadSegmentOrDefault = None
        self.IsDrivingOnRoad = False
        self.Position2f = None
        self.Position3f = None
        self.GroundPositionTile2i = None
        self.GroundPositionTile = None
        self.Direction = None
        self.IsSpawned = False
        self.ForceFlatGround = False
        self.Id = None
        self.DefaultTitle = None
        self.Prototype = None
        self.Context = None
        self.IsDestroyed = False
        self.IsEnabled = False
        self.IsNotEnabled = False
        self.IsPaused = False
        self.IsNotPaused = False
        self.RendererData = None
        self.WorkersNeeded = int(0)
        self.HasWorkersCached = False
        self.MaintenanceCosts = None
        self.IsIdleForMaintenance = False
class TreePlanterJobProvider:

    def __init__(self):
        pass

class TreePlanterState:

    def __init__(self):
        pass

class TreePlanterProto:

    def __init__(self):
        self.EntityType = None
        self.CostToBuild = None
        self.DisruptsSurface = False
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
class Timings:

    def __init__(self):
        pass

class Gfx:

    def __init__(self):
        self.IconPath = str(0)
