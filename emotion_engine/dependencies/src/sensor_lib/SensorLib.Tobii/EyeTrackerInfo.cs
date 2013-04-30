using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Tobii
{

    using TobiiEyeTrackerInfo = global::Tobii.Eyetracking.Sdk.EyetrackerInfo;

    [Serializable]
    class EyeTrackerInfo : IEyeTrackerInfo
    {

        //  Constructors  //

        public EyeTrackerInfo(TobiiEyeTrackerInfo info)
        {
            generation = info.Generation;
            givenName = info.GivenName;
            model = info.Model;
            productId = info.ProductId;
            status = info.Status;
            version = info.Version;
            return;
        }


        //  Public Methods //

        public String Generation { get { return generation; } }
        public String GivenName { get { return givenName; } }
        public String Model { get { return model; } }
        public String ProductId { get { return productId; } }
        public String Status { get { return status; } }
        public String Version { get { return version; } }

        


        //  Overriding Methods  //

        public override bool Equals(object obj)
        {
            EyeTrackerInfo a1 = obj as EyeTrackerInfo;
            if (a1 == null)
                return false;

            return ProductId.Equals(a1.ProductId);
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
        


        //  Private Fields  //

        readonly String generation;
        readonly String givenName;
        readonly String model;
        readonly String productId;
        readonly String status;
        readonly String version;

    }
}
