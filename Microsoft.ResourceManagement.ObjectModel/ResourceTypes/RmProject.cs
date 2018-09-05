using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.ResourceManagement.ObjectModel.ResourceTypes
{
    [Serializable]
     public partial class RmProject:RmResource
    {
 /// <summary>
        /// The type of the wrapped resource.
        /// </summary>
        protected const String ResourceType = @"MaireProject";

        /// <summary>
        /// Gets the FIM name of the wrapped resource type.
        /// </summary>
        /// <returns>The FIM name of the wrapped resource type.</returns>
        public override string GetResourceType() {
            return ResourceType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RmProject()
            : base() {
        }

        /// <summary>
        /// Constructor for serialization.
        /// </summary>
        protected RmProject(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {
        }

        #region promoted properties


        /// <summary>
        /// Account Name
        /// User's log on name
        /// </summary>
        public string ProjectCode {
            get { return GetString(AttributeNames.ProjectCode); }
            set { base[AttributeNames.ProjectCode].Value = value; }
        }

        /// <summary>
        /// AD User Cannot Change Password
        /// Will sync from AD to track whether the user is locked out from changing their AD password
        /// </summary>
        public RmReference ProjectManagerID
        {
            get { return GetReference(AttributeNames.ProjectManagerID); }
            set {  base[AttributeNames.ProjectManagerID].Value= value; }
        }

        
        #endregion

        #region Protected methods

        /// <summary>
        /// Ensures all attributes exist.
        /// </summary>
        protected override void EnsureSpecificAttributesExist() {
            EnsureAttributeExists(AttributeNames.ProjectCode, false);
            EnsureAttributeExists(AttributeNames.ProjectManagerID, false);
            
        }

        /// <summary>
        /// Implement this method to ensure that custom attributes, i.e.
        /// attributes not defined in the default FIM schema, exist.
        /// </summary>
        protected void EnsureCustomAttributesExist(RmAttributeName attributeName, bool multiValued)
        {
            EnsureNotDisposed();
            lock (attributes)
            {
                if (attributeName == null)
                {
                    throw new ArgumentNullException("attributeName");
                }
                if (attributes.ContainsKey(attributeName))
                {
                    return;
                }
                else
                {
                    attributes.Add(attributeName, multiValued ? (RmAttributeValue)new RmAttributeValueMulti() : (RmAttributeValue)new RmAttributeValueSingle());
                }
            }
        }

        private void EnsureNotDisposed()
        {
            if (attributes == null)
            {
                throw new ObjectDisposedException("RmObject", "The RmObject object has already been disposed");
            }
        }
        #endregion

        #region AttributeNames

        /// <summary>
        /// Names of Person specific attributes
        /// </summary>
        public sealed new partial class AttributeNames {
            /// <summary>
            /// Account Name
            /// User's log on name
            /// </summary>
            public static RmAttributeName ProjectCode = new RmAttributeName(@"ProjectCode");
            /// <summary>
            /// AD User Cannot Change Password
            /// Will sync from AD to track whether the user is locked out from changing their AD password
            /// </summary>
            public static RmAttributeName ProjectManagerID = new RmAttributeName(@"ProjectManagerID");
           
        }

        #endregion

    }
}
