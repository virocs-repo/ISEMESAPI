namespace ISEMES.Models
{
    public class CreateShipmentRequest
    {
        // From address
        //public string FromName { get; set; }
        //public string FromStreet1 { get; set; }
        //public string FromCity { get; set; }
        //public string FromState { get; set; }
        //public string FromZip { get; set; }
        //public string FromCountry { get; set; }
        //public string FromEmail { get; set; }
        //public string FromPhone { get; set; }

        // To address
        public string ToName { get; set; }
        public string ToStreet1 { get; set; }
        public string ToCity { get; set; }
        public string ToState { get; set; }
        public string ToZip { get; set; }
        public string ToCountry { get; set; }

        // Parcel details
        //public string ParcelLength { get; set; }
        //public string ParcelWidth { get; set; }
        //public string ParcelHeight { get; set; }
        //public string ParcelWeight { get; set; }

        public int ShipmentID { get; set; }  
        public int UserID { get; set; }

        public string ClientAccountNumber { get; set; }

        public List<ParcelRequestData> Parcels { get; set; }
    }

    public class ParcelRequestData
    {
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string DistanceUnit { get; set; } = "in"; // Default unit

        public decimal Weight { get; set; }
        public string MassUnit { get; set; } = "lb"; // Default unit

        public int Pno { get; set; }
    }

}

