using System.Collections.Generic;

namespace Rasa.Structures
{
    using Data;

    public class EntityClass
    {
        public uint ClassId { get; set; }                           // entityClass.pyo
        public string ClassName { get; set; }                       // entityClass.pyo
        public int MeshId { get; set; }                             // entityClass.pyo
        public int LodMeshId { get; set; }                          // entityClass.pyo for all entityClasses LodMeshId is none (null)
        public double LodScreenPercentage { get; set; }             // entityClass.pyo for all entityClasses LodScreenPercentage is none (null)
        public short ClassCollisionRole { get; set; }               // entityClass.pyo
        public List<AugmentationType> Augmentations { get; set; }   // entityClass.pyo
        public bool TargetFlag { get; set; }                        // entityClass.pyo

        public int SkeletonId { get; set; }
        public int CameraCollideFlag { get; set; }
        public int CullingLayerType { get; set; }
        public int CombinedGroupId { get; set; }
        public int DiscardCombined { get; set; }
        public int CastsShadowFlag { get; set; }
        public int PickableFlag { get; set; }
        public int TargetPickOverride { get; set; }
        public int HasServerSkeleton { get; set; }

        public EntityClass(uint classId, string className, int meshId, short classCollisionRole, List<AugmentationType> augList, bool targetFlag)
        {
            ClassId = classId;
            ClassName = className;
            MeshId = MeshId;
            ClassCollisionRole = classCollisionRole;
            Augmentations = augList;
            TargetFlag = targetFlag;
        }
    }
}
