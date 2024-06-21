using Microsoft.EntityFrameworkCore.Storage;
using PartFinderCore.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PartFinderCore.Classes;

public class DataBaseInit
{
    public static void InitDataBaseValues()
    {
        using var dataContext = new SiteContext();

        if (!dataContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
        {
            dataContext.Database.EnsureCreated();
        }

        AddFootPrintCategories(dataContext);
        AddFootPrints(dataContext);
        AddPartCategories(dataContext);
        AddManufacturers(dataContext);
        dataContext.StorageLocation.Add(new StorageLocation { StorageName = "Box 1", StorageSortOrder = 1 });

        dataContext.SaveChanges();

    }

    private static void AddFootPrintCategories(SiteContext dataContext)
    {
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "Axial" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "BGA" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "CBGA" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "CERDIP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "DFN" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "DIP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "FCBGA" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "HTSSOP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "Misc" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "Other" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "PDIP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "PLCC" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "QFN" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "SOIC" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "SOT" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "SSOP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "SSOT" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "TO" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "TQFP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "TSOP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "TSSOP" });
        dataContext.FootprintCategory.Add(new FootprintCategory { FCName = "VSSOP" });
            
    }

    private static void AddFootPrints(SiteContext dataContext)
    {
        AddFootprint("CBGA-32", "32-Lead Ceramic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("FCBGA-576", "576-Ball Ball Grid Array, Thermally Enhanced", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-119", "119-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-169", "169-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-225", "225-Ball Plastic a Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-260", "260-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-297", "297-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-304", "304-Lead Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-316", "316-Lead Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-324", "324-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-385", "385-Lead Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-400", "400-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-484", "484-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-625", "625-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("PBGA-676", "676-Ball Plastic Ball Grid Array", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("SBGA-256", "256-Ball Ball Grid Array, Thermally Enhanced", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("SBGA-304", "304-Ball Ball Grid Array, Thermally Enhanced", "/docs/footprints/bga.svg", "BGA", dataContext);
        AddFootprint("SBGA-432", "432-Ball Ball Grid Array, Thermally Enhanced", "/docs/footprints/bga.svg", "BGA", dataContext);

        AddFootprint("DFN 8", "", "/docs/footprints/dfn.svg", "DFN", dataContext);

        AddFootprint("CerDIP-8", "8-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-14", "14-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-16", "16-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-18", "18-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-20", "20-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-24 Narrow", "24-Lead Ceramic Dual In-Line Package - Narrow Body", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-24 Wide", "24-Lead Ceramic Dual In-Line Package - Wide Body", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-28", "28-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("CerDIP-40", "40-Lead Ceramic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-8", "8-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-14", "14-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-16", "16-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-18", "18-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-20", "20-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-24", "24-Lead Plastic Dual In-Line Package", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-28 Narrow", "28-Lead Plastic Dual In-Line Package, Narrow Body", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-28 Wide", "28-Lead Plastic Dual In-Line Package, Wide Body", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-40", "", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-6", "", "/docs/footprints/dip.svg", "DIP", dataContext);
        AddFootprint("PDIP-4", "", "/docs/footprints/dip.svg", "DIP", dataContext);


        AddFootprint("QFN 16", "", "/docs/footprints/qfn.svg", "QFN", dataContext);
        AddFootprint("QFN 43", "", "/docs/footprints/qfn.svg", "QFN", dataContext);
        AddFootprint("QFN 40", "", "/docs/footprints/qfn.svg", "QFN", dataContext);
        AddFootprint("QNF-36", "", "/docs/footprints/qfn.svg", "QFN", dataContext);

        AddFootprint("SOIC-8", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-16", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-28 300mil", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-18 300mil", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-16 300mil", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-14", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-24 300mil", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-20 300mil", "", "/docs/footprints/soic.svg", "SOIC", dataContext);
        AddFootprint("SOIC-N-EP-8", "8-Lead Standard Small Outline Package, with Expose Pad", "/docs/footprints/soic.svg", "SOIC", dataContext);

        AddFootprint("SSOT-6", "", "/docs/footprints/sot.svg", "SOT", dataContext);
        AddFootprint("SOT505-1", "", "/docs/footprints/sot.svg", "SOT", dataContext);
        AddFootprint("SOT-23", "", "/docs/footprints/sot.svg", "SOT", dataContext);
        AddFootprint("SOT-5", "", "/docs/footprints/sot.svg", "SOT", dataContext);

        AddFootprint("SSOP-5", "", "/docs/footprints/sot.svg", "SSOT", dataContext);
        AddFootprint("SSOT-5", "", "/docs/footprints/sot.svg", "SSOT", dataContext);
        AddFootprint("SSOT-3", "", "/docs/footprints/sot.svg", "SSOT", dataContext);
        AddFootprint("SSOP-14", "", "/docs/footprints/sot.svg", "SSOT", dataContext);
        AddFootprint("HTSSOP 16", "", "/docs/footprints/sot.svg", "SSOT", dataContext);


        AddFootprint("TQFP-44", "", "/docs/footprints/tqfp.svg", "TQFP", dataContext);
        AddFootprint("TQFP-64", "", "/docs/footprints/tqfp.svg", "TQFP", dataContext);
        AddFootprint("TQFP-100", "", "/docs/footprints/tqfp.svg", "TQFP", dataContext);
        AddFootprint("TQFP-32", "", "/docs/footprints/tqfp.svg", "TQFP", dataContext);


        AddFootprint("TSOP-16", "", "/docs/footprints/soic.svg", "TSOP", dataContext);
        AddFootprint("TSOP-20", "", "/docs/footprints/soic.svg", "TSOP", dataContext);
        AddFootprint("TSOP-14", "", "/docs/footprints/soic.svg", "TSOP", dataContext);
        AddFootprint("TSOP-54", "", "/docs/footprints/soic.svg", "TSOP", dataContext);
        AddFootprint("TSSOP-48", "", "/docs/footprints/soic.svg", "TSOP", dataContext);

        AddFootprint("Axial Component", "", "/docs/footprints/axial.svg", "Axial", dataContext);

        AddFootprint("TO-3", "", "", "TO", dataContext);
        AddFootprint("TO-5", "", "", "TO", dataContext);

        AddFootprint("DIP Module", "", "/docs/footprints/dip.svg", "Other", dataContext);
        AddFootprint("PowerSO-36", "", "/docs/footprints/soic.svg", "Other", dataContext);

        AddFootprint("TSSOP-8", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-12", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-16", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-14", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-20", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-24", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);
        AddFootprint("TSSOP-28", "", "/docs/footprints/soic.svg", "TSSOP", dataContext);

        AddFootprint("VSSOP-8", "", "/docs/footprints/soic.svg", "VSSOP", dataContext);
        AddFootprint("VSSOP-10", "", "/docs/footprints/soic.svg", "VSSOP", dataContext);
        AddFootprint("VSSOP-12", "", "/docs/footprints/soic.svg", "VSSOP", dataContext);

        AddFootprint("HTSSOP-32", "", "/docs/footprints/soic.svg", "HTSSOP", dataContext);
        AddFootprint("TA27A", "", "", "Misc", dataContext);
        AddFootprint("TA25", "", "", "Misc", dataContext);
        AddFootprint("PLCC-20", "", "/docs/footprints/plcc.svg", "PLCC", dataContext);

           
    }


    private static void AddPartCategories(SiteContext dataContext)
    {
        AddPartCategory("Active Components", "", "Root", dataContext);
        AddPartCategory("Passive Components", "", "Root", dataContext);
        AddPartCategory("Electromechanical Parts", "", "Root", dataContext);
        AddPartCategory("Mechanical Parts", "", "Root", dataContext);
        AddPartCategory("Cables", "", "Root", dataContext);
        AddPartCategory("Speakers and Sounders", "", "Root", dataContext);
        AddPartCategory("Modules", "", "Root", dataContext);
        AddPartCategory("Other", "", "Root", dataContext);
        AddPartCategory("Optoelectronics & Displays", "", "Root", dataContext);

        AddPartCategory("Semiconductors - ICs", "", "Active Components", dataContext);
        AddPartCategory("Others", "", "Active Components", dataContext);
        AddPartCategory("Semiconductors - Discretes", "", "Active Components", dataContext);

        AddPartCategory("Resistors", "", "Passive Components", dataContext);
        AddPartCategory("Capactiors", "", "Passive Components", dataContext);
        AddPartCategory("Crystals, Oscillators & Resonators", "", "Passive Components", dataContext);
        AddPartCategory("Potentiometers", "", "Passive Components", dataContext);
        AddPartCategory("Photoresistors", "", "Passive Components", dataContext);
        AddPartCategory("Inductors", "", "Passive Components", dataContext);

        AddPartCategory("Connectors", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Switches", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Relays", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Fuses", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Battery Holders", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Transformers", "", "Electromechanical Parts", dataContext);
        AddPartCategory("Motors", "", "Electromechanical Parts", dataContext);

        AddPartCategory("Washers", "", "Mechanical Parts", dataContext);
        AddPartCategory("Mica Washers", "", "Mechanical Parts", dataContext);
        AddPartCategory("Cooling", "", "Mechanical Parts", dataContext);
        AddPartCategory("Screws", "", "Mechanical Parts", dataContext);
        AddPartCategory("Nuts", "", "Mechanical Parts", dataContext);
        AddPartCategory("Bolts", "", "Mechanical Parts", dataContext);
        AddPartCategory("Pullleys", "", "Mechanical Parts", dataContext);
        AddPartCategory("Bearings", "", "Mechanical Parts", dataContext);
        AddPartCategory("Spacers", "", "Mechanical Parts", dataContext);

        AddPartCategory("RF Modules", "", "Modules", dataContext);
        AddPartCategory("Communications & Networking Modules", "", "Modules", dataContext);
        AddPartCategory("Interface Modules", "", "Modules", dataContext);
        AddPartCategory("MCU Modules", "", "Modules", dataContext);

        AddPartCategory("Logic", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Amplifiers & Comparators", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Audio Processing & Control", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Clock,Timing & Frequency Management", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("CODECs / Encoders / Decoders", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Configurable Mixed Signal ICs", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Data & Signal Conversion", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Digital Potentiometers", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Digital Signal Controllers - DSC", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Digital Signal Processors - DSP", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Drivers & Interfaces", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Filters - Active", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("FPGAs / CPLDs", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("GALs / PALs & SPLDs", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Sensor ICs", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Isolated Feedback Generators", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Memory", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Memory Controllers", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Microcontrollers - MCU", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Microprocessors - MPU", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Network Controllers", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Power Management ICs", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("RF", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Special Function", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Switches, Multiplexers & Demultiplexers", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Touch Screen Controllers", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("TV Tuners", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Video Processing", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Opto-Isolators", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Voltage References", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Digital Isolators", "", "Semiconductors - ICs", dataContext);
        AddPartCategory("Darlington Arrays", "", "Semiconductors - ICs", dataContext);

        AddPartCategory("Displays", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("Fibre Optic Components", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("Infrared Emitters & Receivers", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("Laser Components & Modules", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("LEDs", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("LED Accessories", "", "Optoelectronics & Displays", dataContext);
        AddPartCategory("Photodetectors & Photointerrupters", "", "Optoelectronics & Displays", dataContext);

        AddPartCategory("PIC10", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("PIC12", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("PIC16", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("PIC18", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("PIC24 / DSPIC", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("PIC32", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("ATMEGA", "", "Microcontrollers - MCU", dataContext);
        AddPartCategory("ARM", "", "Microcontrollers - MCU", dataContext);

        AddPartCategory("Diodes", "", "Semiconductors - Discretes", dataContext);
        AddPartCategory("Electron Tubes", "", "Semiconductors - Discretes", dataContext);
        AddPartCategory("Intelligent Power Modules", "", "Semiconductors - Discretes", dataContext);
        AddPartCategory("Power Modules", "", "Semiconductors - Discretes", dataContext);
        AddPartCategory("Thyristors", "", "Semiconductors - Discretes", dataContext);
        AddPartCategory("Transistors", "", "Semiconductors - Discretes", dataContext);

        AddPartCategory("Bridge Rectifier Diodes", "", "Diodes", dataContext);
        AddPartCategory("Current Regulator Diodes", "", "Diodes", dataContext);
        AddPartCategory("Fast Recovery Rectifier Diodes", "", "Diodes", dataContext);
        AddPartCategory("RF / Pin Diodes", "", "Diodes", dataContext);
        AddPartCategory("Schottky Diodes", "", "Diodes", dataContext);
        AddPartCategory("Small Signal Diodes", "", "Diodes", dataContext);
        AddPartCategory("Standard Recovery Rectifier Diodes", "", "Diodes", dataContext);
        AddPartCategory("Zener Diodes", "", "Diodes", dataContext);

        AddPartCategory("Bipolar Transistors", "", "Transistors", dataContext);
        AddPartCategory("IGBT Arrays & Modules", "", "Transistors", dataContext);
        AddPartCategory("IGBT Single Transistors", "", "Transistors", dataContext);
        AddPartCategory("JFET Transistors", "", "Transistors", dataContext);
        AddPartCategory("Miscellaneous Transistors", "", "Transistors", dataContext);
        AddPartCategory("MOSFET Transistors", "", "Transistors", dataContext);
        AddPartCategory("RF FET Transistors", "", "Transistors", dataContext);
        AddPartCategory("RF Transistors", "", "Transistors", dataContext);
        AddPartCategory("Unijunction Transistors - UJT", "", "Transistors", dataContext);

           



    }

    private static void AddManufacturers(SiteContext dataContext)
    {
        AddManufacturer("Microchip / Amtel", "https://www.microchip.com/", dataContext);
        AddManufacturer("Other", "", dataContext);
        AddManufacturer("ADI (Analog Devices Inc.)", "https://www.analog.com/en/index.html", dataContext);
        AddManufacturer("NXP Semiconductors ", "https://www.nxp.com/", dataContext);
        AddManufacturer("Maxim Integrated ", "https://www.maximintegrated.com/en.html", dataContext);
        AddManufacturer("Linear Technology / Analog Devices ", "", dataContext);
        AddManufacturer("STMicroelectronics ", "https://www.st.com/content/st_com/en.html", dataContext);
        AddManufacturer("Texas Instruments ", "https://www.ti.com/", dataContext);
        AddManufacturer("Vishay ", "", dataContext);
        AddManufacturer("Arduino", "", dataContext);
        AddManufacturer("National Semiconductor", "", dataContext);
        AddManufacturer("Samsung", "", dataContext);
        AddManufacturer("Cypress", "", dataContext);
    }

    private static void AddFootprint(string footprintName, string footprintDescription, string footprintImage, string footprintCategory, SiteContext dataContext)
    {
        var post = dataContext.FootprintCategory.FirstOrDefault(y => y.FCName == footprintCategory);
        if (post != null)
        {
            dataContext.Footprint.Add(new Footprint { FootprintName = footprintName, FootprintDescription = footprintDescription, FootprintImage = footprintImage, FootprintCategory = post.FCPkey });
        }
        dataContext.SaveChanges();
    }

    private static void AddPartCategory(string name, string description, string category, SiteContext dataContext)
    {
        if (category == "Root")
        {
            dataContext.PartCategory.Add(new PartCategory { PCName = name, PCDescription = description, ParentID = 0 });
        } else
        {
            var post = dataContext.PartCategory.FirstOrDefault(y => y.PCName == category);
            if (post != null)
            {
                dataContext.PartCategory.Add(new PartCategory { PCName = name, PCDescription = description, ParentID = post.PCpkey });
            }
        }
        dataContext.SaveChanges();
    }
    private static void AddManufacturer(string name, string url, SiteContext dataContext)
    {
        dataContext.Manufacturer.Add(new Manufacturer { ManufacturerName = name, ManufacturerURL = url });
        dataContext.SaveChanges();
    }
}