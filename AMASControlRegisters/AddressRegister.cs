using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using AMAS_DBI;
using ClassPattern;

namespace AMASControlRegisters
{

    //[Designer(typeof(AMASControlRegisters.Design.RegRootDesigner), typeof(IRootDesigner))]

    public partial class AddressRegister : UserControl
    {
        private AMAS_DBI.Class_syb_acc AMASacc;

        public delegate void AlterAddress();
        public event AlterAddress SelectedAddress;

        private Address_ids flat_ids;
        private Address_ids house_ids;
        private Address_ids street_ids;
        private Address_ids city_ids;
        private Address_ids state_ids;
        private Address_ids trc_ids;
        private Address_ids areal_ids;
        private Address_ids district_ids;

        public string state { get { return state_ids.AddressBox.Text.Trim(); } }
        public string trc { get { return trc_ids.AddressBox.Text.Trim(); } }
        public string areal { get { return areal_ids.AddressBox.Text.Trim(); } }
        public string city { get { return city_ids.AddressBox.Text.Trim(); } }
        public string district { get { return district_ids.AddressBox.Text.Trim(); } }
        public string street { get { return street_ids.AddressBox.Text.Trim(); } }
        public string house { get { return house_ids.AddressBox.Text.Trim(); } }
        public string flat { get { return flat_ids.AddressBox.Text.Trim(); } }

        public int stateID { get { return state_ids.get_ident(); } }
        public int trcID { get { return trc_ids.get_ident(); } }
        public int arealID { get { return areal_ids.get_ident(); } }
        public int cityID { get { return city_ids.get_ident(); } }
        public int districtID { get { return district_ids.get_ident(); } }
        public int streetID { get { return street_ids.get_ident(); } }
        public int houseID { get { return house_ids.get_ident(); } }
        public int flatID { get { return flat_ids.get_ident(); } }

        public AddressRegister()
        {
            InitializeComponent();
            //LoadAttrb();
        //}

        //private void LoadAttrb()
        //{
            flat_ids = new Address_ids(this.FlatBox);
            house_ids = new Address_ids(this.HouseBox);
            street_ids = new Address_ids(this.StreetBox);
            district_ids = new Address_ids(this.DistrictBox);
            city_ids = new Address_ids(this.CityBox);
            areal_ids = new Address_ids(this.ArealBox);
            trc_ids = new Address_ids(this.TrcBox);
            state_ids = new Address_ids(this.StateBox);
            state_ids.Child = trc_ids;
            trc_ids.Child = areal_ids;
            areal_ids.Child = city_ids;
            city_ids.Child = district_ids;
            district_ids.Child = street_ids;
            street_ids.Child = house_ids;
            house_ids.Child = flat_ids;
            house_ids.AddressBox.SelectedIndexChanged += new EventHandler(HouseAddressBox_SelectedIndexChanged);
            street_ids.AddressBox.SelectedIndexChanged += new EventHandler(StreetAddressBox_SelectedIndexChanged);
            district_ids.AddressBox.SelectedIndexChanged += new EventHandler(DistrictAddressBox_SelectedIndexChanged);
            city_ids.AddressBox.SelectedIndexChanged += new EventHandler(CityAddressBox_SelectedIndexChanged);
            areal_ids.AddressBox.SelectedIndexChanged += new EventHandler(ArealAddressBox_SelectedIndexChanged);
            trc_ids.AddressBox.SelectedIndexChanged += new EventHandler(TrcAddressBox_SelectedIndexChanged);
            state_ids.AddressBox.SelectedIndexChanged += new EventHandler(StateAddressBox_SelectedIndexChanged);
            flat_ids.AddressBox.LostFocus+=new EventHandler(Flat_LostFocus);
            house_ids.AddressBox.LostFocus+=new EventHandler(House_LostFocus);
            street_ids.AddressBox.LostFocus+=new EventHandler(Street_LostFocus);
            district_ids.AddressBox.LostFocus += new EventHandler(District_LostFocus);
            city_ids.AddressBox.LostFocus += new EventHandler(City_LostFocus);
            areal_ids.AddressBox.LostFocus += new EventHandler(Areal_LostFocus);
            trc_ids.AddressBox.LostFocus += new EventHandler(Trc_LostFocus);
            state_ids.AddressBox.LostFocus += new EventHandler(State_LostFocus);
        }

        public void connect(AMAS_DBI.Class_syb_acc SybAcc)
        {
            AMASacc = SybAcc;
            flat_ids.connect(AMASacc);
            house_ids.connect(AMASacc);
            street_ids.connect(AMASacc);
            district_ids.connect(AMASacc);
            city_ids.connect(AMASacc);
            areal_ids.connect(AMASacc);
            trc_ids.connect(AMASacc);
            state_ids.connect(AMASacc);
            Select_State();
            state_ids.get_index(AMASacc.Default_State);
            Select_Cities();
            city_ids.get_index(AMASacc.Default_City);
            if (AMASCommand.Access == null) AMASCommand.AccessCommands(AMASacc);
        }

        public void State_LostFocus(object sender, EventArgs e)
        {
            string adrname = state_ids.NewAddressName;
            if (state_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_State();
                state_ids.get_index_by_text(adrname);
            }
            try
            {
                SelectedAddress();
            }
            catch { }
        }

        public void City_LostFocus(object sender, EventArgs e)
        {
            string adrname = city_ids.NewAddressName;
            if (city_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Cities();
                city_ids.get_index_by_text(adrname);
             }
                SelectedAddress();
       }

        public void Street_LostFocus(object sender, EventArgs e)
        {
            string adrname = street_ids.NewAddressName;
            if (street_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Streets();
                street_ids.get_index_by_text(adrname);
            }
                 SelectedAddress();
       }

        public void House_LostFocus(object sender, EventArgs e)
        {
            string adrname = house_ids.NewAddressName;
            if (house_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Houses();
                house_ids.get_index_by_text(adrname);
            }
                SelectedAddress();
        }

        public void Flat_LostFocus(object sender, EventArgs e)
        {
            string adrname = flat_ids.NewAddressName;
            if (flat_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Flats();
                flat_ids.get_index_by_text(adrname);
            }
                SelectedAddress();
        }

        public void District_LostFocus(object sender, EventArgs e)
        {
            string adrname = district_ids.NewAddressName;
            if (district_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Districts();
                district_ids.get_index_by_text(adrname);
            }
                SelectedAddress();
        }

        public void Areal_LostFocus(object sender, EventArgs e)
        {
            string adrname = areal_ids.NewAddressName;
            if (areal_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Areals();
                areal_ids.get_index_by_text(adrname);
            }
                SelectedAddress();
        }

        public void Trc_LostFocus(object sender, EventArgs e)
        {
            string adrname = trc_ids.NewAddressName;
            if (trc_ids.NewAddressName.Length > 0)
            {
                add_address();
                Select_Trcs();
                trc_ids.get_index_by_text(adrname);
            }
                SelectedAddress();
        }

        private void add_address()
        {
            string State = "";
            string Trc = "";
            string Areal = "";
            string City = "";
            string District = "";
            string Street = "";
            string House="";
            string Flat="";
            if (state_ids.NewAddressName.Length > 0) State = state_ids.NewAddressName;
            else
            {
                State = state_ids.AddressBox.Text;
                if (trc_ids.NewAddressName.Length > 0) Trc = trc_ids.NewAddressName;
                else
                {
                    Trc = trc_ids.AddressBox.Text;
                    if (areal_ids.NewAddressName.Length > 0) Areal = areal_ids.NewAddressName;
                    else
                    {
                        Areal = areal_ids.AddressBox.Text;
                        if (city_ids.NewAddressName.Length > 0) City = city_ids.NewAddressName;
                        else
                        {
                            City = city_ids.AddressBox.Text;
                            if (district_ids.NewAddressName.Length > 0) District = district_ids.NewAddressName;
                            else
                            {
                                District = district_ids.AddressBox.Text;
                                if (street_ids.NewAddressName.Length > 0) Street = street_ids.NewAddressName;
                                else
                                {
                                    Street = street_ids.AddressBox.Text;
                                    if (house_ids.NewAddressName.Length > 0) House = house_ids.NewAddressName;
                                    else
                                    {
                                        House = house_ids.AddressBox.Text;
                                        if (flat_ids.NewAddressName.Length > 0) Flat = flat_ids.NewAddressName;
                                        else Flat = flat_ids.AddressBox.Text;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AMASCommand.ADD_Address(State, Trc, Areal, City, District, Street, House, Flat);
        }

        public void clear_address()
        {
            city_ids.clear();
        }

        public int get_address()
        {
            return flat_ids.get_ident();
        }

        public void set_address(int flat)
        {
            int state = 0;
            int trc = 0;
            int areal = 0;
            int city = 0;
            int district = 0;
            int street = 0;
            int house = 0;
            try
            {
                if (AMASacc.Set_table("TAddrReg1", AMAS_Query.Class_AMAS_Query.GetAddressIds(flat),null))
                    if (AMASacc.Rows_count > 0)
                    {
                        AMASacc.Get_row(0);
                        state = (int)AMASacc.Find_Field("state");
                        trc = (int)AMASacc.Find_Field("region");
                        areal = (int)AMASacc.Find_Field("areal");
                        city = (int)AMASacc.Find_Field("city");
                        district = (int)AMASacc.Find_Field("district");
                        street = (int)AMASacc.Find_Field("street");
                        house = (int)AMASacc.Find_Field("house");
                        Select_State();
                        state_ids.get_index(state);
                        Select_Trcs();
                        trc_ids.get_index(trc);
                        Select_Areals();
                        areal_ids.get_index(areal);
                        Select_Cities();
                        city_ids.get_index(city);
                        Select_Districts();
                        district_ids.get_index(district);
                        Select_Streets();
                        street_ids.get_index(street);
                        Select_Houses();
                        house_ids.get_index(house);
                        Select_Flats();
                        flat_ids.get_index(flat);
                    }
                    else
                    {
                        state_ids.clear();
                        Select_State();
                    }
                else
                {
                    state_ids.clear();
                    Select_State();
                }
            }
            catch
            {
            }

        }

        private void Select_State()
        {
            state_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetStateList,"state","id");
        }

        private void Select_Cities()
        {
            int ident=areal_ids.get_ident();
            if (ident >= 0)
                city_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetCityList(ident), "name_city", "id");
            else city_ids.clear();
        }

        private void Select_Streets()
        {
            int ident=district_ids.get_ident();
            if (ident >= 0)
                street_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetStreetList(ident), "streetname", "id");
            else street_ids.clear();
        }

        private void Select_Houses()
        {
            int ident=street_ids.get_ident();
            if (ident >= 0)
                house_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetHouseList(ident), "house", "id");
            else house_ids.clear();
        }

        private void Select_Flats()
        {
            int ident=house_ids.get_ident();
            if (ident >= 0)
                flat_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetFlatList(ident), "flat", "id");
            else flat_ids.clear();
        }

        private void Select_Districts()
        {
            int ident = city_ids.get_ident();
            if (ident >= 0)
                district_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetDistrictList(ident), "district", "id");
            else district_ids.clear();
        }

        private void Select_Areals()
        {
            int ident = trc_ids.get_ident();
            if (ident >= 0)
                areal_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetArealList(ident), "areal", "id");
            else areal_ids.clear();
        }

        private void Select_Trcs()
        {
            int ident = state_ids.get_ident();
            if (ident >= 0)
                trc_ids.Select_Subject(AMAS_Query.Class_AMAS_Query.GetTrcList(ident), "Region", "id");
            else trc_ids.clear();
        }

        private void HouseAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Flats();
        }

        private void StreetAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Houses();
        }

        private void CityAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Districts();
        }

        private void StateAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Trcs();
        }

        private void DistrictAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Streets();
        }

        private void ArealAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Cities();
        }

        private void TrcAddressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Areals();
        }

        public string AddressString()
        {
            string str = StateBox.Text.Trim();
            if (TrcBox.Text.Trim().Length > 0) str += ", " + TrcBox.Text.Trim();
            if (ArealBox.Text.Trim().Length > 0) str += ", " + ArealBox.Text.Trim();
            if (CityBox.Text.Trim().Length > 0) str += ", " + CityBox.Text.Trim();
            if (DistrictBox.Text.Trim().Length > 0) str += ", " + DistrictBox.Text.Trim();
            if (StreetBox.Text.Trim().Length > 0) str += ", " + StreetBox.Text.Trim();
            if (HouseBox.Text.Trim().Length > 0) str += ", " + HouseBox.Text.Trim();
            if (FlatBox.Text.Trim().Length > 0) str += ", " + FlatBox.Text.Trim();
            return str;
        }

     }
}
