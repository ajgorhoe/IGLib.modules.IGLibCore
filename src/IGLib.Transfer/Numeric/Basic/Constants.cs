// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

        // MATHEMATICAL AND PHYSICAL CONSTANTS
        // PHYSICAL UNITS
    
        // HERE YOU CAN FIND THINGS THAT DEFINE HOW OUR UNIVERSE LOOKS LIKE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// using MathNet.Numerics;

namespace IG.Num
{

    /// <summary>SI base units (International system of units).</summary>
    public enum SI { 
        /// <summary>Metre, an SI unit for length.</summary>
        m, 
        /// <summary>Kilogram, an SI unit for mass.</summary>
        kg, 
        /// <summary>Second, an SI unit for time.</summary>
        s, 
        /// <summary>Ampere, an SI unit for electric current.</summary>
        A, 
        /// <summary>Kelvin, an SI unit for thermodynamic temperature.</summary>
        K,
        /// <summary>Candela, an SI unit for luminous intensity.</summary>
        cd, 
        /// <summary>Mole, an SI unit for amount of substance.</summary>
        mol 
    }





    /// <summary>Data of a physical constant, including its value, standard error, units, symbol and description.
    /// This is alro used for derived SI units and non-SI units.</summary>
    public struct PhysicalConstant
    {

        // Constructors:

        /// <summary>Creates a new physical constant with specified properties (value, units, description...).</summary>
        /// <param name="value">Value of the constant.</param>
        /// <param name="relativeerror">Standard error of the current constant measurements.</param>
        /// <param name="numerator">List of units in the numerator. Units that are raised to some power are repeated the corresponding number of times.</param>
        /// <param name="denominator">List of units in the denominator.</param>
        /// <param name="symbol">Symbol of the constant.</param>
        /// <param name="name">Name of the constant.</param>
        /// <param name="description">A short one line description of the constant.</param>
        public PhysicalConstant(double value, double relativeerror,
                SI[] numerator, SI[] denominator, string symbol, string name, string description)
        {
            _value = _error = 0;
            _unitsabove = _unitsbelow = null;
            _symbol = _name = _description = null;
            Init(value, relativeerror,numerator, denominator, symbol, name, description);
        }

        /// <summary>Creates a new physical constant with specified properties (value, units, description...).</summary>
        /// <param name="value">Value of the constant.</param>
        /// <param name="relativeerror">Standard error of the current constant measurements.</param>
        /// <param name="numerator">List of units in the numerator. Units that are raised to some power are repeated the corresponding number of times.</param>
        /// <param name="denominator">List of units in the denominator.</param>
        public PhysicalConstant(double value, double relativeerror,
                SI[] numerator, SI[] denominator)
        {
            _value = _error = 0;
            _unitsabove = _unitsbelow = null;
            _symbol = _name = _description = null;
            Init(value, relativeerror,numerator, denominator, null, null, null);
        }

        /// <summary>Auxiliary function for constructors.</summary>
        private void Init(double value, double relativeerror,
                SI[] numerator, SI[] denominator, string symbol, string name, string description)
        {
            _value = value;
            _error = value * relativeerror;
            int num = numerator.Length;
            if (num <= 0)
                _unitsabove = null;
            else
            {
                _unitsabove = new SI[num];
                for (int i = 0; i < num; ++i)
                    _unitsabove[i] = numerator[i];
            }
            num = denominator.Length;
            if (num <= 0)
                _unitsbelow = null;
            else
            {
                _unitsbelow = new SI[num];
                for (int i = 0; i < num; ++i)
                    _unitsbelow[i] = numerator[i];
            }
            _symbol = symbol;
            _name = name;
            _description = description;
        }

        // data:

        private double _value, _error;
        SI[] _unitsabove, _unitsbelow;
        private string _symbol, _name, _description;

        /// <summary>Gets the value of the physical constant.</summary>
        public double Value { get { return _value; } }

        /// <summary>Gets the standard uncertainty of the physical constant.
        /// As constants can be measured more precisley with time, this value is likely subject to changes.</summary>
        public double Error { get { return _error; } }

        /// <summary>Getst the relative standard uncertainty of the physical constant.</summary>
        public double RelativeError { get { return Error / Value; } }

        /// <summary>Gets the array of SI physical units in the nominator of the physical constant.</summary>
        public SI[] UnitsNumerator
        {
            get{ return _unitsabove; }
        }

        /// <summary>Gets the array of SI physical units in the denominator of the physical constant.</summary>
        public SI[] UnitsDenumerator
        {
            get{ return _unitsbelow; }
        }

        /// <summary>Gets the symbol used for a physical constant.</summary>
        public string Symbol { get { return _symbol; } }

        /// <summary>Gets the name of the physical constant.</summary>
        public string Name { get { return _name; } }

        /// <summary>Gets the description of the physical constant.</summary>
        public string Description { get { return _description; } }


        /// <summary>Returns a string that represents units listed in units.</summary>
        private string UnitsString(SI[] units)
        {
            StringBuilder sb = new StringBuilder();
            int m=0, kg=0, s=0, A=0, K=0, cd=0, mol=0; // count numbers of appearances
            int numsymb = 0; // count number of different symbols that appear 
            for (int i=0;i<units.Length;++i)
            {
                switch (units[i])
                {
                    case SI.m:
                        if (m==0)
                            ++numsymb;
                        ++m;
                        break;
                    case SI.kg:
                        if (kg==0)
                            ++ numsymb;
                        ++kg;
                        break;
                    case SI.s:
                        if (s==0)
                            ++numsymb;
                        ++s;
                        break;
                    case SI.A:
                        if (A==0)
                            ++numsymb;
                        ++A;
                        break;
                    case SI.K:
                        if (K==0)
                            ++numsymb;
                        ++K;
                        break;
                    case SI.cd:
                        if (cd==0)
                            ++numsymb;
                        ++cd;
                        break;
                    case SI.mol:
                        if (mol==0)
                            ++numsymb;
                        ++mol;
                        break;
                    default:
                        throw new Exception("Composition of constant with units: unit " 
                            + UnitsNumerator[i].ToString() + " not implemented.");
                }
                if(numsymb==0)
                    sb.Append("1");
                else
                {
                    if (m>0)
                    {
                        sb.Append(SI.m.ToString());
                        if (m>1) { sb.Append("^"); sb.Append(m.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (kg>0)
                    {
                        sb.Append(SI.kg.ToString());
                        if (kg>1) { sb.Append("^"); sb.Append(kg.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (s>0)
                    {
                        sb.Append(SI.s.ToString());
                        if (s>1) { sb.Append("^"); sb.Append(s.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (A>0)
                    {
                        sb.Append(SI.A.ToString());
                        if (A>1) { sb.Append("^"); sb.Append(A.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (K>0)
                    {
                        sb.Append(SI.K.ToString());
                        if (K>1) { sb.Append("^"); sb.Append(K.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (cd>0)
                    {
                        sb.Append(SI.cd.ToString());
                        if (cd>1) { sb.Append("^"); sb.Append(cd.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (mol>0)
                    {
                        sb.Append(SI.mol.ToString());
                        if (mol>1) { sb.Append("^"); sb.Append(mol.ToString());}
                        --numsymb;
                        if (numsymb>0)
                            sb.Append("·");
                    }
                    if (numsymb>0)
                        throw new Exception("Composition of constant with units: not all symbols have been worked. This is probably a bug.");
                }
            }
            return sb.ToString();
        }


        /// <summary>Returns a string representation of a physical constant.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            int num = 0, numden=0;
            if (UnitsNumerator!=null)
                num=UnitsNumerator.Length;
            if (UnitsDenumerator!=null)
                numden=UnitsDenumerator.Length;
            sb.Append(Value.ToString());
            if (num>0 || numden>0)
            {
                if (numden>0)
                    sb.Append("(");
                if (num==0)
                    sb.Append("·1");
                else
                {
                    sb.Append(UnitsString(UnitsNumerator));
                }
                if (numden>0)
                {
                    sb.Append("/");
                    if (numden>1)
                        sb.Append("(");
                    sb.Append(UnitsString(UnitsDenumerator));
                    if (numden>1)
                        sb.Append(")");
                }
                if (numden>0)
                    sb.Append(")");
            }
            return sb.ToString();
        }


    } // class PhysicalConstant 
     


    /// <summary>Mathematical and physical constants.</summary>
    public static class ConstMath
    {

        // Mathematical constants:

        /// <summary>Archimedes' constant or Ludolph'result number, 
        /// the ratio of any circle'result circumference to its diameter in Euclidean space,
        /// also the ratio of a circle'result area to the square of its radius.</summary>
        public const double Pi = 3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706d;

        /// <summary>Euler'result number (or Napier'result constant), base of Natural logarithm.</summary>
        public const double E = 2.718281828459045235360287471352662497757247093699959574966967627724076630353547594571382178525166427d;

        /// <summary>The golden ratio, (1+Sqr(5))/2;
        /// the ratio of two quantities such that the ratio between the sum of those quantities and the larger one 
        /// is the same as the ratio between the larger one and the smaller one.
        /// a/s = (a+s)/a (a>s)</summary>
        public const double GoldenRatio = 1.618033988749894848204586834365638117720309179805762862135448622705260462818902449707207204189391137d;

        /// <summary>Square root of 2.</summary>
        public const double Sqr2 = 1.414213562373095048801688724209698078569671875376948073176679737990732478462107038850387534327641573d;

        /// <summary>Square root of 3.</summary>
        public const double Sqr3 = 1.732050807568877293527446341505872366942805253810380628055806979451933016908800037081146186757248576d;

        /// <summary>Square root of 5.</summary>
        public const double Sqr5 = 2.236067977499789696409173668731276235440618359611525724270897245410520925637804899414414408378782275d;

        /// <summary>Laplace limit,
        /// the maximum value of the eccentricity for which the series solution to Kepler'result equation converges.</summary>
        public const double LaplaceLimit = 0.66274341934918158097474209710925290d;

        /// <summary>googol, more for fun than for serious work :)</summary>
        public const double googol = 10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000d;


        // Complex constants:

        ///// <summary>Imaginary unit (Complex).</summary>
        //public static readonly Complex i = new MathNet.Numerics.Complex(0, 1);

        ///// <summary>Imaginary unit (Complex).</summary>
        //public static readonly Complex ImaginaryUnit = i;

        ///// <summary>Complex zero.</summary>
        //public static readonly Complex ComplexZero = new MathNet.Numerics.Complex(0, 0);

        ///// <summary>Complex unit for multiplication (1).</summary>
        //public static readonly Complex ComplexOne = new MathNet.Numerics.Complex(1, 0);



        // Physical constants:


        //public static PhysicalConstant ElectronMass = new PhysicalConstant{ 0.1, 0.2 };

    }  // class Const


    /// <summary>Physical constants including units and standard uncertainty.</summary>
    /// <remarks><para>See also:</para>
    /// <para>Wikipedia - Physical constants http://en.wikipedia.org/wiki/Physical_constant </para>
    /// <para>Units and constants handbook, http://www.knowledgedoor.com/2/units_and_constants_handbook/index.html </para></remarks>
    public static partial class ConstPhysical
    {

        // TODO: Add symbols and desctiptions to the constant representations (just copy from comments).

        // Universal physical constants:

        /// <summary>Speed of light in vacuum (set by definition of meter).
        /// <para>Unit: m/s</para></summary>
        public static readonly PhysicalConstant LightSpeed = new PhysicalConstant(
            299792458, 0  /* constant is set by definition of meter that relies on the definition of second and the speed of light */,
            new SI[] { SI.m },
            new SI[] { SI.s } );


        /// <summary>Gravitational constant (set by definition of meter).
        /// <para>Unit: m^3/(kg s^2)</para></summary>
        public static readonly PhysicalConstant GravitationalConstant = new PhysicalConstant(
            6.67428e-11, 1.0e-4 ,
            new SI[] { SI.m, SI.m, SI.m },
            new SI[] { SI.kg, SI.s, SI.s } );


        /// <summary>Planck'result constant,
        /// ratio between energy of a photon and the frequency of its associated electromagnetic wave.
        /// <para>Unit: kg m^2/(s).</para></summary>
        public static readonly PhysicalConstant PlanckConstant = new PhysicalConstant(
            6.62606896e-34d, 5.0e-8,
            new SI[] { SI.kg, SI.m, SI.m },
            new SI[] { SI.s } );

        /// <summary>Planck'result constant divided by 2 Pi (used when angular frequency is used).
        /// <para>Unit: kg m^2/(s).</para></summary>
        public static readonly PhysicalConstant ReducedPlanckConstant = new PhysicalConstant(
            1.054571628e-34d, 5.0e-8,
            new SI[] { SI.kg, SI.m, SI.m },
            new SI[] { SI.s } );

        /// <summary>Magnetic constant μ0, vacuum permeability.
        /// <para>Unit: m*kg/(s^2 A^2).</para></summary>
        public static readonly PhysicalConstant MagneticConstant = new PhysicalConstant(
            // 1.2566370614359172953e-6, 
            4*Math.PI*1.0e-7,
            0,
            new SI[] { SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.A, SI.A } );


        /// <summary>Electric constant ε0, vacuum permittivity.
        /// <para>Unit:F/m = A^2*s^4/(kg*m^3)</para></summary>
        public static readonly PhysicalConstant ElectricConstant = new PhysicalConstant(
            8.854187817e-12, 0,
            new SI[] { SI.s, SI.s, SI.s, SI.s, SI.A, SI.A },
            new SI[] { SI.kg, SI.m, SI.m, SI.m } );

        /// <summary>CoulombConstant. 1/(4*Pi*Epsilon0)
        /// <para>Unit: m/F = kg*m^3/(A^2*s^4).</para></summary>
        public static readonly PhysicalConstant CoulombConstant = new PhysicalConstant(
            8.9875517874e9, 0,
            new SI[] { SI.kg, SI.m, SI.m, SI.m },
            new SI[] { SI.s, SI.s, SI.s, SI.s, SI.A, SI.A } );

        /// <summary>Elementary electric charge, the electric charge of a proton.
        /// <para>Unit: C = A*s.</para></summary>
        public static readonly PhysicalConstant ElementaryCharge = new PhysicalConstant(
            1.602176487e-19, 2.5e-8,
            new SI[] { SI.s, SI.A },
            null );

        /// <summary>Characteristic impendance of vacuum, 
        /// ratio of magnitudes of magnitudes of the electric and magnetic fields in electromagnetic radiation.
        /// <para>Unit: Ω = kg*m^2/(s^3*A^2)</para></summary>
        public static readonly PhysicalConstant VacuumImpendance = new PhysicalConstant(
            376.730313461, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.s, SI.A, SI.A } );

        /// <summary>Bohr Magenton.
        /// <para>Unit: J/T = m^2*A.</para></summary>
        public static readonly PhysicalConstant BohrMagneton = new PhysicalConstant(
            9.27400915e-24, 2.5e-8,
            new SI[] { SI.m, SI.m, SI.A },
            null );

        /// <summary>Nuclear Magneton.
        /// <para>Unit: J/T = m^2*A.</para></summary>
        public static readonly PhysicalConstant NuclearMagneton = new PhysicalConstant(
            5.05078343e-27, 8.6e-8,
            new SI[] { SI.m, SI.m, SI.A },
            new SI[] { } );

        // TODO: Insert conductance quantum, Josephson constant, magnetic flux quantum, von Klitzing constant.


        // Atomic and nuclear constants:
          
          
        /// <summary>Atomic mass unit,
        /// 1/12 of the mass of an unbound atom of carbon-12 at rest and in its ground state.
        /// <para>Unit: kg.</para></summary>
        public static readonly PhysicalConstant AtomicMassUnit = new PhysicalConstant(
            1.66053886e-27, 1.7e-7,
            new SI[] { SI.kg },
            null );
          
        /// <summary>Avogadro's number,
        /// the number of atoms in exactly 12 grams of carbon-12.
        /// Unit: none.</summary>
        public static readonly PhysicalConstant AvogadroNumber = new PhysicalConstant(
            6.0221415e23, 1.7e-7,
            null,
            null );
          
        /// <summary>Boltzmann constant, gas constant divided by the Avogadro constant.
        /// Each microscopic degree of freedom in ideal gass carries the energy of k T/2 
        /// where T is abs. temperature and k is Boltzmann constant.
        /// <para>Unit: J/K = m^2*kg/(s^2*K).</para></summary>
        public static readonly PhysicalConstant BoltzmannConstant = new PhysicalConstant(
            1.3806505e-23, 1.8e-6,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.K } );

        /// <summary>.Faraday constant, 
        /// the magnitude of electric charge per mole of electrons.
        /// <para>Unit: C/mol = A*s/mol.</para></summary>
        public static readonly PhysicalConstant FaradayConstant = new PhysicalConstant(
            96485.3383, 8.6e-8,
            new SI[] { SI.s, SI.A },
            new SI[] { SI.mol } );

        /// <summary>Stefan'result constant (or Bolzmann-Stefan constant).
        /// <para>Unit: W/m^2 K^4 = kg/(s^3*K).</para></summary>
        public static readonly PhysicalConstant StefanConstant = new PhysicalConstant(
            5.670400e-8, 7.0e-6,
            new SI[] { SI.kg },
            new SI[] { SI.s, SI.s, SI.s, SI.K, SI.K, SI.K, SI.K } );
        
        /// <summary>Wien'result displacement constant,
        /// proportional constant of the inverse relationship between the wavelength of the peak of the emission 
        /// of a black body and its temperature. 
        /// <para>Unit: m*K.</para></summary>
        public static readonly PhysicalConstant WienDisplacementConstant = new PhysicalConstant(
            2.8977685e-3, 1.7e-6,
            new SI[] { SI.m, SI.K },
            new SI[] {  } );

        

        /// <summary>Bohr Radius.
        /// <para>Unit: m.</para></summary>
        public static readonly PhysicalConstant BohrRadius = new PhysicalConstant(
            5.2917720859e-11, 3.3e-9,
            new SI[] { SI.m },
            new SI[] { } );

         
        /// <summary>Electron mass.
        /// Unit: kg.</summary>
        public static readonly PhysicalConstant ElectronMass = new PhysicalConstant(
            9.10938215e-31, 5.0e-8,
            new SI[] { SI.kg },
            null );

        /// <summary>Proton mass.
        /// Unit: kg.</summary>
        public static readonly PhysicalConstant ProtonMass = new PhysicalConstant(
            1.672621637e-26, 5.0e-8,
            new SI[] { SI.kg },
            new SI[] { } );

        /// <summary>Neutron mass.
        /// Unit: kg.</summary>
        public static readonly PhysicalConstant NeutronMass = new PhysicalConstant(
            1.67492729e-27, 5.0e-8,
            new SI[] { SI.kg },
            new SI[] { } );
          
        /// <summary>Fine-structure constant,
        /// a coupling constant characterizing the strength of the electromagnetic interaction.
        /// Unit: /.</summary>
        public static readonly PhysicalConstant FineStructureConstant = new PhysicalConstant(
            7.2973525376e-3, 6.8e-10,
            new SI[] { } ,
            new SI[] { } );

        /// <summary>Rydberg constant,
        /// wavenumber of the lowest-energy photon capable of ionizing the hydrogen atom from its ground state.
        /// <para>Unit: 1/m.</para></summary>
        public static readonly PhysicalConstant RydbergConstant = new PhysicalConstant(
            10973731.568525, 6.6e-12,
            new SI[] { },
            new SI[] { SI.m } );


        /// <summary>Classical electron radius.
        /// Unit: numrows.</summary>
        public static readonly PhysicalConstant ElectronRadius = new PhysicalConstant(
            2.8179402894e-15, 2.1e-9,
            new SI[] { SI.m },
            null );
          
        /// <summary>Hartree energy,
        /// the absolute value of the electric potential energy of the hydrogen atom in its ground state.
        /// <para>Unit: J = m^2*kg/s^2.</para></summary>
        public static readonly PhysicalConstant HartreeEnergy = new PhysicalConstant(
            4.35974417e-18, 1.7e-7,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s } );

        /// <summary>Thomson cross section.
        /// <para>Unit: 1/m^2.</para></summary>
        public static readonly PhysicalConstant ThomsonCrossection = new PhysicalConstant(
            0.665245873e-28, 2.0e-8,
            null,
            new SI[] { SI.m, SI.m } );

        /// <summary>Weak mixing angle.
        /// <para>Unit: none.</para></summary>
        public static readonly PhysicalConstant WeakMixingAngle = new PhysicalConstant(
            0.22215, 3.4e-3,
            null, null );


        /// <summary>Standard gravity acceleration, 
        /// nominal acceleration due to gravity at the Earth'result surface at sea level.
        /// <para>Unit: m/s^2.</para></summary>
        public static readonly PhysicalConstant StandardGravity = new PhysicalConstant(
            9.80665, 0,
            new SI[] { SI.m },
            new SI[] { SI.s, SI.s } );

          
        /// <summary>Standard atmosphere,
        /// nominal air pressure at Earth surface, by definition.
        /// <para>Unit: Pa =  N/m^2 = kg/(m*s^2).</para></summary>
        public static readonly PhysicalConstant StandardAtmosphere = new PhysicalConstant(
            101325, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s } );
         


        /* Template:
        /// <summary>.
        /// Unit: .</summary>
        public static PhysicalConstant  = new PhysicalConstant(
            , ,
            new SI[] {  },
            new SI[] {  } );
          */


    }  //  class ConstPhysical


    /// <summary>SI prefixes for producing multiples of the original units (such as kilo- or micro-).</summary>
    public static class SIPrefix
    {
        // Factors greater or equal to one:


        /// <summary>Factor of 1 (1), 10^0.</summary>
        public static readonly PhysicalConstant one = new PhysicalConstant(
            1, 0,
            null,
            null,
            "1", "one",
            "One, the factor 1.");

        /// <summary>deca- (da), SI prefix for 10^1.</summary>
        public static readonly PhysicalConstant da = new PhysicalConstant(
            10, 0,
            null,
            null,
            "da", "deca",
            "deca-, the SI prefix for factor of 10^1.");

        /// <summary>hecto- (h), SI prefix for 10^2.</summary>
        public static readonly PhysicalConstant h = new PhysicalConstant(
            100, 0,
            null,
            null,
            "h", "hecto",
            "hecto- (h), the SI prefix for factor of 10^2.");
         
        /// <summary>kilo- (k), SI prefix for 10^3.</summary>
        public static readonly PhysicalConstant k = new PhysicalConstant(
            1000, 0,
            null,
            null,
            "k", "kilo",
            "kilo- (k), the SI prefix for factor of 10^3.");
         
        /// <summary>mega- (M), SI prefix for 10^6.</summary>
        public static readonly PhysicalConstant M = new PhysicalConstant(
            1e6, 0,
            null,
            null,
            "M", "mega",
            "mega- (M), the SI prefix for factor of 10^6.");
         
        /// <summary>giga- (G), SI prefix for 10^9.</summary>
        public static readonly PhysicalConstant G = new PhysicalConstant(
            1e9, 0,
            null,
            null,
            "G", "giga",
            "giga- (), the SI prefix for factor of 10^9.");
         
        /// <summary>tera- (T), SI prefix for 10^12.</summary>
        public static readonly PhysicalConstant T = new PhysicalConstant(
            1e12, 0,
            null,
            null,
            "T", "tera",
            "tera- (T), the SI prefix for factor of 10^12.");
         
        /// <summary>peta- (P), SI prefix for 10^15.</summary>
        public static readonly PhysicalConstant P = new PhysicalConstant(
            1e15, 0,
            null,
            null,
            "P", "peta",
            "peta- (P), the SI prefix for factor of 10^15.");
         
        /// <summary>exa- (E), SI prefix for 10^18.</summary>
        public static readonly PhysicalConstant E = new PhysicalConstant(
            1e18, 0,
            null,
            null,
            "E", "exa",
            "exa- (E), the SI prefix for factor of 10^18.");
         
        /// <summary>zetta- (Z), SI prefix for 10^21.</summary>
        public static readonly PhysicalConstant Z = new PhysicalConstant(
            1e21, 0,
            null,
            null,
            "Z", "zetta",
            "zetta- (Z), the SI prefix for factor of 10^21.");
         
        /// <summary>yotta- (Y), SI prefix for 10^.</summary>
        public static readonly PhysicalConstant Y = new PhysicalConstant(
            1e24, 0,
            null,
            null,
            "Y", "yotta",
            "yotta- (Y), the SI prefix for factor of 10^24.");



         
        /// <summary>deci- (l), SI prefix for 10^-1.</summary>
        public static readonly PhysicalConstant d = new PhysicalConstant(
            1e-1, 0,
            null,
            null,
            "d", "deci",
            "deci- (d), the SI prefix for factor of 10^-1.");
         
        /// <summary>centi- (c), SI prefix for 10^-2.</summary>
        public static readonly PhysicalConstant c = new PhysicalConstant(
            1e-2, 0,
            null,
            null,
            "c", "centi",
            "centi- (c), the SI prefix for factor of 10^-2.");
         
        /// <summary>milli- (numrows), SI prefix for 10^-3.</summary>
        public static readonly PhysicalConstant m = new PhysicalConstant(
            1e-3, 0,
            null,
            null,
            "m", "milli",
            "milli- (m), the SI prefix for factor of 10^-3.");
         
        /// <summary>micro- (µ), SI prefix for 10^-6.</summary>
        public static readonly PhysicalConstant micro = new PhysicalConstant(
            1e-6, 0,
            null,
            null,
            "µ", "micro",
            "micro- (µ), the SI prefix for factor of 10^-6.");
         
        /// <summary>nano- (d2), SI prefix for 10^-9.</summary>
        public static readonly PhysicalConstant n = new PhysicalConstant(
            1e-9, 0,
            null,
            null,
            "n", "nano",
            "nano- (n), the SI prefix for factor of 10^-9.");
         
        /// <summary>pico- (p), SI prefix for 10^-12.</summary>
        public static readonly PhysicalConstant p = new PhysicalConstant(
            1e-12, 0,
            null,
            null,
            "p", "pico",
            "pico- (p), the SI prefix for factor of 10^-12.");
         
        /// <summary>femto- (f), SI prefix for 10^-15.</summary>
        public static readonly PhysicalConstant f = new PhysicalConstant(
            1e-15, 0,
            null,
            null,
            "f", "femto",
            "femto- (f), the SI prefix for factor of 10^-15.");
         
        /// <summary>atto- (a), SI prefix for 10^-18.</summary>
        public static readonly PhysicalConstant a = new PhysicalConstant(
            1e-18, 0,
            null,
            null,
            "a", "atto",
            "atto- (a), the SI prefix for factor of 10^-18.");
         
        /// <summary>zepto- (z), SI prefix for 10^-.</summary>
        public static readonly PhysicalConstant z = new PhysicalConstant(
            1e-21, 0,
            null,
            null,
            "z", "zepto",
            "zepto- (z), the SI prefix for factor of 10^-21.");
         
        /// <summary>yocto- (y), SI prefix for 10^-24.</summary>
        public static readonly PhysicalConstant y = new PhysicalConstant(
            1e-24, 0,
            null,
            null,
            "y", "yocto",
            "yocto- (y), the SI prefix for factor of 10^-24.");

    } // class SIPrefix


    /// <summary>SI units (basic and derived)</summary>
    public static partial class PhysicalUnit
    {

        // SI basic units:

        /// <summary>Metre, the basic SI unit of length.</summary>
        public static readonly PhysicalConstant m = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m },
            null,
            "m", "meter", 
            "Meter, the SI basic unit of length." );

        /// <summary>Kilogram, the basic SI unit of mass.</summary>
        public static readonly PhysicalConstant kg = new PhysicalConstant(
            1, 0,
            new SI[] { SI.kg },
            null,
            "kg", "kilogram", 
            "Kilogram, the SI basic unit of mass." );

        /// <summary>Second, the basic SI unit of time.</summary>
        public static readonly PhysicalConstant s = new PhysicalConstant(
            1, 0,
            new SI[] { SI.s },
            null,
            "s", "second",
            "Second, the SI basic unit of time.");

        /// <summary>Ampere, the basic SI unit of electric current.</summary>
        public static readonly PhysicalConstant A = new PhysicalConstant(
            1, 0,
            new SI[] { SI.A },
            null,
            "A", "ampere",
            "Ampere, the SI basic unit of electric current.");

        /// <summary>Kelvin, the basic SI unit of temperature.</summary>
        public static readonly PhysicalConstant K = new PhysicalConstant(
            1, 0,
            new SI[] { SI.K },
            null,
            "K", "kelvin",
            "Kelvin, the SI basic unit of temperature.");

        /// <summary>Candela, the basic SI unit of 	luminous intensity.</summary>
        public static readonly PhysicalConstant cd = new PhysicalConstant(
            1, 0,
            new SI[] { SI.cd },
            null,
            "cd", "candela",
            "Candela, the SI basic unit of luminous intensity.");

        /// <summary>Mole, the basic SI unit of amount of substance.</summary>
        public static readonly PhysicalConstant mol = new PhysicalConstant(
            1, 0,
            new SI[] { SI.cd },
            null,
            "mol", "mole",
            "Mole, the SI basic unit of amount of substance.");


        // Derived SI units:

        /// <summary>Converts temperature in Kelvins to temperature in Celsius.</summary>
        /// <param name="Kelvin">Temperature in K.</param>
        /// <returns>Temperature in C.</returns>
        public static double CelsiusFromKelvins(double Kelvin)
        {
            return Kelvin - 273.15;
        }

        /// <summary>Converts temperature in Celsius to temperature in Kelvins.</summary>
        /// <param name="Celsius">Temperature in degrees Celsius.</param>
        /// <returns>Temperature in K.</returns>
        public static double KelvinFromCelsius(double Celsius)
        {
            return Celsius + 273.15;
        }

        /// <summary>Hertz, the SI derived unit of frequency.
        /// 1/result.</summary>
        public static readonly PhysicalConstant Hz = new PhysicalConstant(
            1, 0,
            null,
            new SI[] { SI.s },
            "Hz", "hertz",
            "Hertz, SI derived unit of frequency [1/s].");

        /// <summary>Radian, the SI derived unit of angle.
        /// Plane angle at which the circle arc length defined by this angle equals circle radius,
        /// 180/Pi degrees.</summary>
        public static readonly PhysicalConstant rad = new PhysicalConstant(
            1, 0,
            null,
            null,
            "rad", "radian",
            "Radian, SI derived unit of angle.");

        /// <summary>Steradian, the SI derived unit of solid angle.
        /// Solid angle at which the portion of the sphere surface defined by this angle has the area of square radius.</summary>
        public static readonly PhysicalConstant sr = new PhysicalConstant(
            1, 0,
            null,
            null,
            "sr", "steradian",
            "Steradian, SI derived unit of angle.");


        /// <summary>Newton, the SI derived unit of force.
        /// N = numrows kg/result^2.</summary>
        public static readonly PhysicalConstant N = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.kg },
            new SI[] { SI.s, SI.s },
            "N", "newton",
            "N, SI derived unit of force [m kg&s^2].");

        /// <summary>Pascal, the SI derived unit of pressure or stress.
        /// Pa = N/numrows^2 = kg/numrows result^2.</summary>
        public static readonly PhysicalConstant Pa = new PhysicalConstant(
            1, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "Pa", "pascal",
            "Pascal, SI derived unit of pressure or stress [N/m^2 = kg/(m s^2)].");

        /// <summary>Joule, the SI derived unit of energy, work, heat.
        /// J = N numrows = numrows^2∙kg/result^2</summary>
        public static readonly PhysicalConstant J = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s },
            "J", "joule",
            "Joule, SI derived unit of energy, work, heat [J = N m = m^2∙kg/s^2].");

        /// <summary>Watt, the SI derived unit of power, radiant flux.
        /// W = J/result = N numrows/result = numrows^2 kg/result^3.</summary>
        public static readonly PhysicalConstant W = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.s, },
            "W", "watt",
            "Watt, SI derived unit of power, radiant flux [W = J/s = N m/s = m^2 kg/s^3].");

        /// <summary>Coulomb, the SI derived unit of electric charge, electric flux.
        /// C = A result.</summary>
        public static readonly PhysicalConstant C = new PhysicalConstant(
            1, 0,
            new SI[] { SI.A, SI.s },
            null,
            "C", "coulomb",
            "Coulomb, SI derived unit of electric charge, electric flux [C = A s].");

        /// <summary>Volt, the SI derived unit of voltage, electric potential difference.
        /// V = J/(A result) = numrows^2 kg/(result^3 A)</summary>
        public static readonly PhysicalConstant V = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.s, SI.A },
            "V", "volt",
            "Volt, SI derived unit of voltage, electric potential difference [V = J/(A s) = m^2 kg/(s^3 A)].");

        /// <summary>Farad, the SI derived unit of electric capacitance.
        /// F = C/V = result^4 A^2/(numrows^2 kg)</summary>
        public static readonly PhysicalConstant F = new PhysicalConstant(
            1, 0,
            new SI[] { SI.s, SI.s, SI.s, SI.s, SI.A, SI.A },
            new SI[] { SI.m, SI.m, SI.kg },
            "F", "farad",
            "Farad, SI derived unit of electric capacitance [F = C/V = s^4 A^2/(m^2 kg)].");


        /// <summary>Ohm, the SI derived unit of electric resistance, impedance.
        /// Ω = V/A = numrows^2 kg/(result^3 A^2)</summary>
        public static readonly PhysicalConstant ohm = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.s, SI.A, SI.A },
            "Ω", "ohm",
            "Ohm, SI derived unit of [Ω = V/A = m^2 kg/(s^3 A^2)].");

        /// <summary>Siements, the SI derived unit of electrical conductance.
        /// S = 1/Ω = result^3 A^2/(numrows^2 kg)</summary>
        public static readonly PhysicalConstant S = new PhysicalConstant(
            1, 0,
            new SI[] { SI.s, SI.s, SI.s, SI.A, SI.A },
            new SI[] { SI.m, SI.m, SI.kg },
            "S", "siemens",
            "Siemens, SI derived unit of electric conductance.");



       /// <summary>Weber, the SI derived unit of magnetic flux.
       /// Wb = J/A = numrows^2 kg/(result^2 A)</summary>
        public static readonly PhysicalConstant Wb = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.A },
            "Wb", "weber",
            "Weber, SI derived unit of magnetic flux [Wb = J/A = m^2 kg/(s^2 A)].");

        /// <summary>Tesla, the SI derived unit of magnetic field.
        /// T = V result/numrows^2 = Wb/numrows^2 = N/(A numrows) = kg/(result^2 A)</summary>
        public static readonly PhysicalConstant T = new PhysicalConstant(
            1, 0,
            new SI[] { SI.kg },
            new SI[] { SI.s, SI.s, SI.A },
            "T", "tesla",
            "Tesla, SI derived unit of magnetic field [T = V s/m^2 = Wb/m^2 = N/(A m) = kg/(s^2 A)].");

        /// <summary>Henry, the SI derived unit of inductance.
        /// H = V result/A = Wb/A = numrows^2 kg/(result^2 A^2)</summary>
        public static readonly PhysicalConstant H = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s, SI.A, SI.A },
            "H", "henry",
            "Henry, SI derived unit of inductance [H = V s/A = Wb/A = m^2 kg/(s^2 A^2)].");

        /// <summary>Lumen, the SI derived unit of luminous flux.
        /// lm = cd sr = cd</summary>
        public static readonly PhysicalConstant lm = new PhysicalConstant(
            1, 0,
            new SI[] { SI.cd },
            null,
            "lm", "lumen",
            "Lumen, SI derived unit of luminous flux [lm = cd sr = cd].");

        /// <summary>Lux, the SI derived unit of illuminance.
        /// lx = lm/(numrows^2) = cd/(numrows^2)</summary>
        public static readonly PhysicalConstant lx = new PhysicalConstant(
            1, 0,
            new SI[] { SI.cd },
            new SI[] { SI.m, SI.m },
            "lx", "lux",
            "Lux, SI derived unit of illuminance [lx = lm/(m^2) = cd/(m^2)].");

        /// <summary>Becquerel, the SI derived unit of radioactivity (decays per unit time).
        /// Bq = 1/result.</summary>
        public static readonly PhysicalConstant Bq = new PhysicalConstant(
            1, 0,
            null,
            new SI[] { SI.s },
            "Bq", "becquerel",
            "Becquerel, SI derived unit of radioactivity (decays per unit time) [1/s].");

        /// <summary>Gray, the SI derived unit of absorbed dose of ionizing radiation.
        /// Gy = J/kg = numrows^2/result^2.</summary>
        public static readonly PhysicalConstant Gy = new PhysicalConstant(
            1, 0,
            new SI[] { SI.m, SI.m },
            new SI[] { SI.s, SI.s },
            "Gy", "gray",
            "Gray, SI derived unit of absorbed dose of ionizing radiation [Gy = J/kg = m^2/s^2].");

        /// <summary>Sievert, the SI derived unit of equivalent dose of ionizing radiation.
        /// Sv = J/kg = numrows^2/result^2.</summary>
        public static readonly PhysicalConstant Sv = new PhysicalConstant(
            1, 0,
             new SI[] { SI.m, SI.m },
            new SI[] { SI.s, SI.s },
            "Sv", "sievert",
            "Sievert, SI derived unit of equivalent dose of ionizing radiation [Sv = J/kg = m^2/s^2].");

        /// <summary>Katal, the SI derived unit of 	catalytic activity.
        /// kat = mol/result.</summary>
        public static readonly PhysicalConstant kat = new PhysicalConstant(
            1, 0,
            new SI[] { SI.mol },
            new SI[] { SI.s },
            "kat", "latal",
            "Katal, SI derived unit of catalytic activity [kat = mol/s].");

        // NON-SI UNITS THAT ARE ACCEPTED FOR USE WITH SI (like hour, day, degree of arc, litre)

        /// <summary>Minute, non-SI unit of time, 60 result.</summary>
        public static readonly PhysicalConstant min = new PhysicalConstant(
            60, 0,
            new SI[] { SI.s },
            null,
            "min", "minute",
            "Minute (min), non-SI unit of time [min = 60 s].");

        /// <summary>Hour, non-SI unit of time, 3600 result.</summary>
        public static readonly PhysicalConstant h = new PhysicalConstant(
            3600, 0,
            new SI[] { SI.s },
            null,
            "h", "hour",
            "Hour (h), non-SI unit of time [h = 60 min = 3600 s].");

        /// <summary>Day, non-SI unit of time, 24 h = 1440 min = 86400 result.</summary>
        public static readonly PhysicalConstant d = new PhysicalConstant(
            86400, 0,
            new SI[] { SI.s },
            null,
            "d", "day",
            "Day (d), non-SI unit of time [d = 24 h = 1440 min = 86400 s].");


        /// <summary>Degree of arc, non-SI unit of plane angle, degarc = 1° = (π/180) rad.</summary>
        public static readonly PhysicalConstant degarc = new PhysicalConstant(
            0.017453292519943295769, 0,
            null,
            null,
            "°", "degree_of_arc",
            "Degree of arc (°), non-SI unit of angle [° = (π/180) rad].");

        /// <summary>Minute of arc, non-SI unit of plane angle, 1′ = (1/60)° = (π/10800) rad.</summary>
        public static readonly PhysicalConstant minarc = new PhysicalConstant(
            0.00029088820866572159615, 0,
            null,
            null,
            "′", "minute_of_arc",
            "Minute of arc (′), non-SI of plane angle [′ = (1/60)° = (π/10800) rad].");

        /// <summary>Second of arc, non-SI unit of plane angle, ″ = (1/60)′ = (1/3600)° = (π/648000) rad.</summary>
        public static readonly PhysicalConstant secarc = new PhysicalConstant(
            4.8481368110953599359e-6, 0,
            null,
            null,
            "″", "second_of_arc",
            "Second of arc (″), non-SI of plane angle [″ = (1/60)′ = (1/3600)° = (π/648000) rad].");

        /// <summary>Square degree, non-SI unit of solid angle, deg2 = (π/180) sr.</summary>
        public static readonly PhysicalConstant deg2 = new PhysicalConstant(
            0.017453292519943295769, 0,
            null,
            null,
            "symbol", "name",
            "Square degree (deg2), non-SI unit of solid angle [deg2 = (π/180) sr].");

         
        /// <summary>Hectare, non-SI unit of area, ha = 100 a = 10000 numrows^2.</summary>
        public static readonly PhysicalConstant ha = new PhysicalConstant(
            1e4, 0,
            new SI[] { SI.m, SI.m },
            null,
            "ha", "hectare",
            "Hectare (ha), non-SI unit of area [ha = 10000 m^2].");
         
        /// <summary>Litre, non-SI unit of volume, l = dm^3 = 0.001 numrows^3.</summary>
        public static readonly PhysicalConstant l = new PhysicalConstant(
            1e-3, 0,
            new SI[] { SI.m, SI.m, SI.m },
            null,
            "l", "litre",
            "Litre (l), non-SI unit of volume [l = dm^3 = 0.001 m^3].");
         
        /// <summary>Tonne, non-SI unit of mass, t = 1000 kg.</summary>
        public static readonly PhysicalConstant t = new PhysicalConstant(
            1e3, 0,
            new SI[] { SI.kg },
            null,
            "t", "tonne",
            "Tonne (t), non-SI unit of mass [t = 1000 kg].");

        /// <summary>Electronvolt, non-SI unit of energy, eV = 1.60217653e−19 J = 1.60217653e−19 numrows^2∙kg/result^2.</summary>
        public static readonly PhysicalConstant eV = new PhysicalConstant(
            1.60217653e-19, 2.5e-8,
            new SI[] { SI.m, SI.m, SI.kg },
            new SI[] { SI.s, SI.s },
            "eV", "electronvolt",
            "Electronvolt (eV), non-SI unit of energy [eV = 1.60217653e−19 J = 1.60217653e−19 m^2∙kg/s^2].");

        /// <summary>Atomic mass unit, non-SI unit of mass, u = 1.66053886e-27 kg.</summary>
        public static readonly PhysicalConstant u = new PhysicalConstant(
            1.66053886e-27, 1.7e-7,
            new SI[] { SI.kg },
            null,
            "u", "atomic_mass_unit",
            "Atomic mass unit (u), non-SI unit of mass [u = 1.66053886e-27 kg].");

        /// <summary>Astronomical unit, non-SI unit of length, AU = 1.49597870691e11 numrows.
        /// Average distance from Sun to Earth.</summary>
        public static readonly PhysicalConstant AU = new PhysicalConstant(
            1.49597870691e11, 1.7e-7,
            new SI[] { SI.m },
            null,
            "AU", "astronomical_unit",
            "Astronomical unit (AU), non-SI unit of length, av. distance Sun-Earth [1.49597870691e11 m].");



        /*  Tenplate:
         * 
         
        /// <summary>, non-SI unit of , .</summary>
        public static readonly PhysicalConstant  = new PhysicalConstant(
            , 0,
            new SI[] { SI. },
            new SI[] { SI. },
            "symbol", "name",
            " (), non-SI unit of [].");


        */
 
    }  // class PhysicalUnit


    /// <summary>Non-SI units whose use is not encouraged or not allowed</summary>
    public static partial class NonSIUnit
    {

        // Units whose use is not encouraged: 

        /// <summary>Ångström, non-SI unit of length, An = 0.1 nm = 10^-10 numrows.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant An = new PhysicalConstant(
            1e-10, 0,
            new SI[] { SI.m },
            null,
            "An", "Ångström",
            "Ångström (An), non-SI unit of length [An = 0.1 nm = 10^-10 m].");

        /// <summary>Nautical mile, non-SI unit of length, mile = 1852 numrows.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant mile = new PhysicalConstant(
            1852, 0,
            new SI[] { SI.m },
            null,
            "mile", "nautical_mile",
            "Nautical mile (mile), non-SI unit of length [mile = 1852 m].");

        /// <summary>Knot, non-SI unit of speed, knot = mile/h = 1852 numrows/3600 result = 0.514444444... numrows/result.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant knot = new PhysicalConstant(
            0.51444444444444444444, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "knot", "knot",
            "Knot (knot), non-SI unit of speed [knot = mile/h = 1852 m/3600 s = 0.514444444... m/s].");

        /// <summary>Are, non-SI unit of area, a = 100 numrows^2.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant a = new PhysicalConstant(
            100, 0,
            new SI[] { SI.m, SI.m },
            null,
            "a", "are",
            "Are (a), non-SI unit of area [a = 100 m^2].");

        /// <summary>Barn, non-SI unit of area, s = 10^-28 numrows^2.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant b = new PhysicalConstant(
            1e-28, 0,
            new SI[] { SI.m, SI.m },
            null,
            "b", "barn",
            "Barn (b), non-SI unit of area [a = 100 m^2].");


        /// <summary>Bar, non-SI unit of pressure, bar = 10^5 Pa = 10^5 kg/numrows result^2.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant bar = new PhysicalConstant(
            10^5, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "bar", "bar",
            "Bar (bar), non-SI unit of pressure [bar = 10^5 Pa = 10^5 kg/m s^2].");

        /// <summary>Millibar, non-SI unit of pressure, mbar = 100 Pa = 100 kg/numrows result^2.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant mbar = new PhysicalConstant(
            10^5, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "mbar", "millibar",
            "Millibar (mbar), non-SI unit of pressure [mbar = 100 Pa = 100 kg/m s^2].");

        /// <summary>Atmosphere, non-SI unit of pressure, atm = 1.01325*10^5 Pa = 1.01325*10^5 kg/numrows result^2.
        /// Use is not encouraged.</summary>
        public static readonly PhysicalConstant atm = new PhysicalConstant(
            1.01325e5, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "atm", "atmosphere",
            "Atmosphere (atm), non-SI unit of pressure [atm = 1.01325*10^5 Pa = 1.01325*10^5 kg/m s^2].");


        // Units whose use is deprecated:

        // Deprecated units - length:

        /// <summary>Inch, a deprecated non-SI unit of length, in = 0.0254 numrows.</summary>
        public static readonly PhysicalConstant inch = new PhysicalConstant(
            0.0254, 0,
            new SI[] { SI.m },
            null,
            "inch", "inch",
            "Inch (inch), deprecated non-SI unit of length [in = 0.0254 m].");
         
        /// <summary>Yard, a deprecated non-SI unit of length, yd = 0.9144 numrows.</summary>
        public static readonly PhysicalConstant yard = new PhysicalConstant(
            0.9144, 0,
            new SI[] { SI.m },
            null,
            "yd", "yard",
            "Yard (yd), deprecated non-SI unit of length [yd = 0.9144 m].");

        /// <summary>Foot, a deprecated non-SI unit of length, ft = 0.30480 numrows.</summary>
        public static readonly PhysicalConstant ft = new PhysicalConstant(
            0.30480, 0,
            new SI[] { SI.m },
            null,
            "ft", "foot",
            "Foot (ft), deprecated non-SI unit of length [ft = 0.30480 m].");

        // Deprecated units - pressure:


        /// <summary>Torr, a deprecated non-SI unit of pressure (mm Hg), torr = 133.322 Pa (Pa = kg/(numrows result^2)).</summary>
        public static readonly PhysicalConstant Torr = new PhysicalConstant(
            133.322, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "Torr", "torr",
            "Torr (Torr), deprecated non-SI unit of pressure [torr = 133.322 Pa].");

        /// <summary>Pound per square inch (psi), a deprecated non-SI unit of pressure, psi = 6894.76 Pa (Pa = kg/(numrows result^2)).</summary>
        public static readonly PhysicalConstant psi = new PhysicalConstant(
            6894.76, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "psi", "pound_per_square_inch",
            "Pound per square inch (psi), deprecated non-SI unit of pressure [psi = 6894.76 Pa].");

        /// <summary>Technical atmosphere, a deprecated non-SI unit of pressure, at = 98066.5 Pa (Pa = kg/(numrows result^2)),
        /// one kilogram-force per square centimeter.</summary>
        public static readonly PhysicalConstant at = new PhysicalConstant(
            98066.5, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.s, SI.s },
            "at", "technical_atmosphere",
            "Technical atmosphere (at), deprecated non-SI unit of pressure [at = 98066.5 Pa].");


        //TODO: Add more units that are deprecated!

        /*  Tenplate:
         * 
         
        /// <summary>, non-SI unit of , .</summary>
        public static readonly PhysicalConstant  = new PhysicalConstant(
            , 0,
            new SI[] { SI. },
            new SI[] { SI. },
            "symbol", "name",
            " (), non-SI unit of [].");


        */



    }

    /// <summary>Miscellaneous constants.</summary>
    public static partial class ConstMisc
    {

        // Astronomic constants:

         
        /// <summary>Light year, distance travelled by light through vacuum in 1 year.</summary>
        public static readonly PhysicalConstant LightYear = new PhysicalConstant(
            9.4607304725808e15, 0,
            new SI[] { SI.m },
            null,
            "ly", "LightYear",
            "Light year, distance travelled by light through vacuum in 1 year [m].");
         
 

        // Earth-related constanst:

        /// <summary>Earth mean radius, 6371.0 km.</summary>
        public static readonly PhysicalConstant EarthRadius = new PhysicalConstant(
            6371.0e3, 0,
            new SI[] { SI.m },
            null,
            "EarthRadius", "EarthRadius",
            "Earth mean radius [= 6371.0e3 m].");

        /// <summary>Earth equatorial radius, 6,378.1 km.</summary>
        public static readonly PhysicalConstant EarthEquatorialRadius = new PhysicalConstant(
            6378.1e3, 0,
            new SI[] { SI.m },
            null,
            "EarthEauatorialRadius", "EarthEauatorialRadius",
            "Earth equatorial radius [= 6378.1e3 m].");

        /// <summary>Earth polar radius, 6,356.8 km.</summary>
        public static readonly PhysicalConstant EarthPolarRadius = new PhysicalConstant(
            6.3568e6, 0,
            new SI[] { SI.m },
            null,
            "EarthPolarRadius", "EarthPolarRadius",
            "Earth polar radius [= 6356.8e3 m].");

        /// <summary>Earth flattening, 0.0033528.
        /// Versine of the spheroid'result angular eccentricity, (a-s)/a.</summary>
        public static readonly PhysicalConstant EarthFlattening = new PhysicalConstant(
            0.0033528, 0,
            null,
            null,
            "EarthFlattening", "EarthFlattening",
            "Earth flattening [= 0.0033528].");

        /// <summary>Earth surface area, 510,072,000 km^2.</summary>
        public static readonly PhysicalConstant EarthSurfaceArea = new PhysicalConstant(
            510.072e12, 0,
            new SI[] { SI.m, SI.m },
            null,
            "EarthSurfaceArea", "EarthSurfaceArea",
            "Earth surface area [= 510,072,000 km^2].");

        /// <summary>Earth surface area of land, 148,940,000 km^2.</summary>
        public static readonly PhysicalConstant EarthSurfaceAreaLand = new PhysicalConstant(
            148.940e12, 0,
            new SI[] { SI.m, SI.m },
            null,
            "EarthSurfaceAreaLand", "EarthSurfaceAreaLand",
            "Earth surface area of land [= 148,940,000 km^2].");

        /// <summary>Earth surface area of sea, 361,132,000 km^2.</summary>
        public static readonly PhysicalConstant EarthSurfaceAreaSea = new PhysicalConstant(
            361.132e12, 0,
            new SI[] { SI.m, SI.m },
            null,
            "EarthSurfaceAreaSea", "EarthSurfaceAreaSea",
            "Earth surface area of sea [= 361,132,000 km^2].");

        /// <summary>Earth volume, 1.0832073e12 km^3.</summary>
        public static readonly PhysicalConstant EarthVolume = new PhysicalConstant(
            1.0832073e21, 0,
            new SI[] { SI.m, SI.m, SI.m },
            null,
            "EarthVolume", "EarthVolume",
            "Earth volume [= 1.0832073e12 km^3].");

        /// <summary>Earth mass, 5.9736e24 kg.</summary>
        public static readonly PhysicalConstant EarthMass = new PhysicalConstant(
            5.9736e24, 0,
            new SI[] { SI.kg },
            null,
            "EarthMass", "EarthMass",
            "Earth mass [= 5.9736e24 kg].");

        /// <summary>Earth averge density, 5.5153e3 kg/numrows^3.</summary>
        public static readonly PhysicalConstant EarthDensity = new PhysicalConstant(
            5.5153e3, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.m, SI.m },
            "EarthDensity", "EarthDensity",
            "Earth density [= 5.5153e3 kg/m^3].");

        /// <summary>Earth equatorial surface gravity acceleration, 9.780327 numrows/result^2.</summary>
        public static readonly PhysicalConstant EarthEquatorialGravity = new PhysicalConstant(
            9.780327, 0,
            new SI[] { SI.m },
            new SI[] { SI.s, SI.s },
            "EarthEquatorialGravity", "EarthEquatorialGravity",
            "Earth equatorial surface gravity [= 9.780327 m/s^2].");

        /// <summary>Earth escape velocity, 11.186e3 numrows/result.
        /// Speed where the kinetic energy of an object is equal to the magnitude of its gravitational 
        /// potential energy at Earth surface.</summary>
        public static readonly PhysicalConstant EarthEscapeVelocity = new PhysicalConstant(
            11.186e3, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "EarthEscapeVelocity", "EarthEscapeVelocity",
            "Earth escape velocity [= 11.186e3 m/s].");

        /// <summary>Earth rotation period, 0.99726968 l = 23h 56min 4.1 result = 86164.1 result.</summary>
        public static readonly PhysicalConstant EarthRotationPeriod = new PhysicalConstant(
            86164.1, 0,
            new SI[] { SI.s },
            null,
            "EarthRotationPeriod", "EarthRotationPeriod",
            "Earth rotation period [= 0.99726968 d = 23h 56min 4.1 s = 86164.1 s].");

        /// <summary>Earth axial tilt, 23.439281°.</summary>
        public static readonly PhysicalConstant EarthAxialTilt = new PhysicalConstant(
            0.409093, 0,
            null,
            null,
            "EarthAxialTilt", "EarthAxialTilt tilt",
            "Earth axial tilt [= 23.439281° = 0.409093 rd].");

        /// <summary>Earth albedo, 0.367.</summary>
        public static readonly PhysicalConstant EarthAlbedo = new PhysicalConstant(
            0.367, 0,
            null,
            null,
            "EarthAlbedo", "EarthAlbedo",
            "Earth albedo [= 0.367].");

        // Earth orbital characteristics:

        /// <summary>Earth aphelion - greatest distance to the center of attraction, 152,097,701 km.</summary>
        public static readonly PhysicalConstant EarthAphelion = new PhysicalConstant(
            152.097701e9, 0,
            new SI[] { SI.m },
            null,
            "EarthAphelion", "EarthAphelion",
            "Earth aphelion - greatest distance to the center of attraction [= 152,097,701 km].");

        /// <summary>Earth perihelion - least distance to the center of attraction, 147,098,074 km.</summary>
        public static readonly PhysicalConstant EarthPerihelion = new PhysicalConstant(
            147.098074e9, 0,
            new SI[] { SI.m },
            null,
            "EarthPerihelion", "EarthPerihelion",
            "Earth perihelion - least distance to the center of attraction [= 147,098,074 km].");

        /// <summary>Earth Sun distance - average distance to Sun (cent. mass), 149,597,870.691 km.</summary>
        public static readonly PhysicalConstant EarthSunDistance = new PhysicalConstant(
            149.597870691e9, 0,
            new SI[] { SI.m },
            null,
            "EarthSunDistance", "EarthSunDistance",
            "Average Earth to Sun distance[= 149,597,870.691 km km].");

        /// <summary>Earth orbit excentricity 0.016710219.</summary>
        public static readonly PhysicalConstant EarthOrbitExcentricity = new PhysicalConstant(
            0.016710219, 0,
            null,
            null,
            "EarthOrbitExcentricity", "EarthOrbitExcentricity",
            "Earth orbit excentricity [= 0.016710219].");

        /// <summary>Earth orbital period - 365.256366 l = 3.15582e7s.</summary>
        public static readonly PhysicalConstant EarthOrbitalPeriod = new PhysicalConstant(
            3.15582e7, 0,
            new SI[] { SI.s },
            null,
            "EarthOrbitalPeriod", "EarthOrbitalPeriod",
            "Earth orbital period [= 365.256366 d = 3.15582e7s].");

        /// <summary>Earth average orbital speed - 29.783 km/result.</summary>
        public static readonly PhysicalConstant EarthOrbitalSpeed = new PhysicalConstant(
            29.783e3, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "EarthOrbitalSpeed", "EarthOrbitalSpeed",
            "Earth average orbital speed [= 29.783 km/s].");



        // Moon - related constants:


        /// <summary>Moon mean radius, 1,737.10 km</summary>
        public static readonly PhysicalConstant MoonRadius = new PhysicalConstant(
            1737.10e3, 0,
            new SI[] { SI.m },
            null,
            "MoonRadius", "MoonRadius",
            "Moon mean radius [= 1,737.10 km].");

        /// <summary>Moon equatorial radius, 1,738.14 km.</summary>
        public static readonly PhysicalConstant MoonEquatorialRadius = new PhysicalConstant(
            1738.14e3, 0,
            new SI[] { SI.m },
            null,
            "MoonEauatorialRadius", "MoonEauatorialRadius",
            "Moon equatorial radius [= 1,738.14 km].");

        /// <summary>Moon polar radius, 1,735.97 km.</summary>
        public static readonly PhysicalConstant MoonPolarRadius = new PhysicalConstant(
            1735.97e3, 0,
            new SI[] { SI.m },
            null,
            "MoonPolarRadius", "MoonPolarRadius",
            "Moon polar radius [= 1,735.97 km].");

        /// <summary>Moon flattening, 0.00125.
        /// Versine of the spheroid'result angular eccentricity, (a-s)/a.</summary>
        public static readonly PhysicalConstant MoonFlattening = new PhysicalConstant(
            0.00125, 0,
            null,
            null,
            "MoonFlattening", "MoonFlattening",
            "Moon flattening [= 0.00125].");

        /// <summary>Moon mass, 7.3477e22 kg.</summary>
        public static readonly PhysicalConstant MoonMass = new PhysicalConstant(
            7.3477e22, 0,
            new SI[] { SI.kg },
            null,
            "MoonMass", "MoonMass",
            "Moon mass [= 7.3477e22 kg].");

        /// <summary>Moon averge density, 3346.4 kg/numrows^3.</summary>
        public static readonly PhysicalConstant MoonDensity = new PhysicalConstant(
            3346.4, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.m, SI.m },
            "MoonDensity", "MoonDensity",
            "Moon density [= 3346.4 kg/m^3].");

        /// <summary>Moon equatorial surface gravity acceleration, 1.622 numrows/result^2.</summary>
        public static readonly PhysicalConstant MoonEquatorialGravity = new PhysicalConstant(
            1.622, 0,
            new SI[] { SI.m },
            new SI[] { SI.s, SI.s },
            "MoonEquatorialGravity", "MoonEquatorialGravity",
            "Moon equatorial surface gravity [= 1.622 m/s^2].");

        /// <summary>Moon escape velocity, 2380 numrows/result.
        /// Speed where the kinetic energy of an object is equal to the magnitude of its gravitational 
        /// potential energy at Moon surface.</summary>
        public static readonly PhysicalConstant MoonEscapeVelocity = new PhysicalConstant(
            2380, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "MoonEscapeVelocity", "MoonEscapeVelocity",
            "Moon escape velocity [= 2380 m/s].");

        /// <summary>Moon rotation period, 27.321582 l = 2.36058e6 result.</summary>
        public static readonly PhysicalConstant MoonRotationPeriod = new PhysicalConstant(
            2.36058e6, 0,
            new SI[] { SI.s },
            null,
            "MoonRotationPeriod", "MoonRotationPeriod",
            "Moon rotation period [= 27.321582 d = 2.36058e6 s].");

        /// <summary>Moon axial tilt (relative to orbital plane), 6.687° = 0.11671 rd.</summary>
        public static readonly PhysicalConstant MoonAxialTilt = new PhysicalConstant(
            0.11671, 0,
            null,
            null,
            "MoonAxialTilt", "MoonAxialTilt tilt",
            "Moon axial tilt ro orbital plane [= 6.687° = 0.11671 rd].");

        /// <summary>Moon average albedo, 0.12.</summary>
        public static readonly PhysicalConstant MoonAlbedo = new PhysicalConstant(
            0.12, 0,
            null,
            null,
            "MoonAlbedo", "MoonAlbedo",
            "Moon albedo [= 0.12].");


        // Moon orbital characteristics:

        /// <summary>Moon aphelion - greatest distance to the center of attraction, 363,104 km.</summary>
        public static readonly PhysicalConstant MoonPerigee = new PhysicalConstant(
            363.104e6, 0,
            new SI[] { SI.m },
            null,
            "MoonPerigee", "MoonPerigee",
            "Moon perigee - least distance to the center of attraction [= 363,104 km].");

        /// <summary>Moon apogee - least distance to the center of attraction, 405,696 km.</summary>
        public static readonly PhysicalConstant MoonApogee = new PhysicalConstant(
            405.696e6, 0,
            new SI[] { SI.m },
            null,
            "MoonApogee", "MoonApogee",
            "Moon apogee - greatest distance to the center of attraction [= 405,696 km].");

        /// <summary>Moon Earth distance - average distance to Earth (cent. mass), 384,400 km.</summary>
        public static readonly PhysicalConstant MoonEarthDistance = new PhysicalConstant(
            384.400e6, 0,
            new SI[] { SI.m },
            null,
            "MoonEarthDistance", "MoonEarthDistance",
            "Average Moon to Earth distance[= 384,400 km].");

        /// <summary>Moon orbit excentricity, 0.0549.</summary>
        public static readonly PhysicalConstant MoonOrbitExcentricity = new PhysicalConstant(
            0.0549, 0,
            null,
            null,
            "MoonOrbitExcentricity", "MoonOrbitExcentricity",
            "Moon orbit excentricity [= 0.0549].");

        /// <summary>Moon orbital period, 27.321582 l = 27 l 7 h 43.1 min = 2.36058*10^6 result.</summary>
        public static readonly PhysicalConstant MoonOrbitalPeriod = new PhysicalConstant(
            2.36058e6, 0,
            new SI[] { SI.s },
            null,
            "MoonOrbitalPeriod", "MoonOrbitalPeriod",
            "Moon orbital period [= 27.321582 d = 27 d 7 h 43.1 min = 2.36058*10^6 s].");

        /// <summary>Moon average orbital speed - 1.022 km/result.</summary>
        public static readonly PhysicalConstant MoonOrbitalSpeed = new PhysicalConstant(
            1.022e3, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "MoonOrbitalSpeed", "MoonOrbitalSpeed",
            "Moon average orbital speed [= 1.022 km/s].");


        // Sun: 

        /// <summary>Sun mean radius, 6.955e8 numrows.</summary>
        public static readonly PhysicalConstant SunRadius = new PhysicalConstant(
            6.955e8, 0,
            new SI[] { SI.m },
            null,
            "SunRadius", "SunRadius",
            "Sun mean radius [= 6.955e8 m].");

        /// <summary>Sun flattening, 9e-6.
        /// (a-s)/a.</summary>
        public static readonly PhysicalConstant SunFlattening = new PhysicalConstant(
            9e-6, 0,
            null,
            null,
            "SunFlattening", "SunFlattening",
            "Sun flattening [= 9e-6].");

        /// <summary>Sun mass, 1.9891e30 kg.</summary>
        public static readonly PhysicalConstant SunMass = new PhysicalConstant(
            1.9891e30, 0,
            new SI[] { SI.kg },
            null,
            "SunMass", "SunMass",
            "Sun mass [= 1.9891e30 kg].");

        /// <summary>Sun averge density, 1408 kg/numrows^3.</summary>
        public static readonly PhysicalConstant SunDensity = new PhysicalConstant(
            1408, 0,
            new SI[] { SI.kg },
            new SI[] { SI.m, SI.m, SI.m },
            "SunDensity", "SunDensity",
            "Sun density [=  1408 kg/m^3].");

        /// <summary>Sun equatorial surface gravity acceleration, 274.0 numrows/result^2.</summary>
        public static readonly PhysicalConstant SunEquatorialGravity = new PhysicalConstant(
            274.0, 0,
            new SI[] { SI.m },
            new SI[] { SI.s, SI.s },
            "SunEquatorialGravity", "SunEquatorialGravity",
            "Sun equatorial surface gravity [= 274.0 m/s^2].");

        /// <summary>Sun escape velocity, 617.7 km/result.
        /// Speed where the kinetic energy of an object is equal to the magnitude of its gravitational 
        /// potential energy at Sun surface.</summary>
        public static readonly PhysicalConstant SunEscapeVelocity = new PhysicalConstant(
            617.7e3, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "SunEscapeVelocity", "SunEscapeVelocity",
            "Sun escape velocity [=  617.7 km/s].");


        /// <summary>Sun temperature, 5778.0 K.</summary>
        public static readonly PhysicalConstant SunSurfaceTemperature = new PhysicalConstant(
            5778.0, 0,
            new SI[] { SI.m },
            new SI[] { SI.s },
            "SunSurfaceTemperature", "SunSurfaceTemperature",
            "Sun surface temperature [= 5778.0 K].");


        // earth Earth earth Earth earth Earth

        /*
        
        /// <summary>.</summary>
        public static readonly PhysicalConstant  = new PhysicalConstant(
            , 0,
            new SI[] { SI. },
            new SI[] { SI. },
            "symbol", "name",
            " [].");

        /// <summary>.</summary>
        public static readonly PhysicalConstant  = new PhysicalConstant(
            , 0,
            new SI[] { SI. },
            new SI[] { SI. },
            "symbol", "name",
            " [].");

        */

        // Solar system related constants:



        // TODO: add more astronomic constants
        
        /* Template:

        /// <summary>.</summary>
        public static readonly PhysicalConstant  = new PhysicalConstant(
            , 0,
            new SI[] { SI. },
            new SI[] { SI. },
            "symbol", "name",
            " [].");
         
        */ 


    }



}
