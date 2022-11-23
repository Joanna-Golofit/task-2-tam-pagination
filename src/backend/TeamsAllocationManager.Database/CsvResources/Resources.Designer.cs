
namespace TeamsAllocationManager.Database.CsvResources;

using System;
    
    
/// <summary>
///   Klasa zasobu wymagaj¹ca zdefiniowania typu do wyszukiwania zlokalizowanych ci¹gów itd.
/// </summary>
// Ta klasa zosta³a automatycznie wygenerowana za pomoc¹ klasy StronglyTypedResourceBuilder
// przez narzêdzie, takie jak ResGen lub Visual Studio.
// Aby dodaæ lub usun¹æ sk³adow¹, edytuj plik ResX, a nastêpnie ponownie uruchom narzêdzie ResGen
// z opcj¹ /str lub ponownie utwórz projekt VS.
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
internal class Resources {
        
    private static global::System.Resources.ResourceManager resourceMan;
        
    private static global::System.Globalization.CultureInfo resourceCulture;
        
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Resources() {
    }
        
    /// <summary>
    /// Zwraca buforowane wyst¹pienie ResourceManager u¿ywane przez tê klasê.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Resources.ResourceManager ResourceManager {
        get {
            if (object.ReferenceEquals(resourceMan, null)) {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TeamsAllocationManager.Database.CsvResources.Resources", typeof(Resources).Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }
        
    /// <summary>
    ///   Przes³ania w³aœciwoœæ CurrentUICulture bie¿¹cego w¹tku dla wszystkich
    ///   przypadków przeszukiwania zasobów za pomoc¹ tej klasy zasobów wymagaj¹cej zdefiniowania typu.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Globalization.CultureInfo Culture {
        get {
            return resourceCulture;
        }
        set {
            resourceCulture = value;
        }
    }
        
    /// <summary>
    /// Wyszukuje zlokalizowany ci¹g podobny do ci¹gu Sala;m2;iloœæ miejsc w pokoju;iloœæ osób pracuj¹cych w pokoju;wolne biurka w pokoju;wolne biurka do wykorzystania prze FP;;
    ///Budynek F1;;;;;;;
    ///F1 001;33,1;8;8;0;0;;
    ///F1 002;25,2;6;2;4;6;;
    ///F1 003;26,9;6;2;4;6;;
    ///F1 004;25,2;5;5;0;0;;
    ///F1 005 ;33,1;6;6;0;0;; 
    ///Piêtro.;;;;;;;
    ///F1 101 ;33,1;9;9;;;;
    ///F1 102;25,2;6;6;0;0;;
    ///F1 103;26,9;6;6;0;0;;
    ///F1 104;25,2;6;6;0;0;;
    ///F1 105;33,1;5;5;0;0;;
    ///F1 106;23,9;2;2;0;0;;
    ///F1 108 ;11,5;;;;;;
    ///F1 110 ;16,5;4;5;0;0;;
    ///F1 111 ;33,1;4;3;0;0;;
    ///Budynek F1;;73;65;8;12;;
    ///;;;; [obciêto pozosta³¹ czêœæ ci¹gu]&quot;;.
    /// </summary>
    internal static string Budynki_FP_pokoje_2019_07 {
        get {
            return ResourceManager.GetString("Budynki_FP_pokoje_2019_07", resourceCulture);
        }
    }
}
