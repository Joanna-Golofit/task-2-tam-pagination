# TeamsAllocationManager

TeamsAllocationManager (T.A.M.) jest wewnętrzną aplikacją służącą do zarządzania dostępnymi pomieszczeniami w firmie.

## Backend

Aplikacja po stronie serwera została zbudowana z  użyciem .net core'a 3.1 w oparciu o Azure Functions.
W tym przypadku pojedyncza funkcja pełni rolę kontrolera, w którym definiowane są endpointy.

Chcąc zdefiniować nowy endpoint, należy dodać funkcję(http trigger, auth level anonymous):

![](/images/readmd/newfunc.png)

W stworzonym pliku należy dokonać następujących modyfikacji:
- usunąć modyfikatory statyczne z metody i klasy
- dodać wspierane metody http w atrybucie HttpTrigger lub nie przekazywać żadnych, by obsługiwać wszystkie zapytania
- podać ścieżkę w atrybucie route, domyślnie "<nazwa encji>/{*path}", gdzie path to nazwa zmiennej dodaje jako parametr funkcji
- odziedziczyć klasę bazową FunctionBase
- wywołać metodę bazową RunAsync w funkcji
- możliwe jest wstrzyknięcie dodatkowych zależności przez konstruktor, dodawanie zależności w pliku startup.cs

Przykładowa funkcja:

![](/images/readmd/funcRoom.png)

Obsługa zapytań przez funkcję dokonywana jest przez zaimplementowanie niestatycznych, publicznych metod opatrzonych atrybutem dziedziczącym po HttpMethodAttribute, np. HttpGetAttribute, HttpPostAttribute lub zaczynającym się od nazwy obsługiwanej metody, poniżej przykład dla powyżej zadeklarowanej funkcji RoomFunction:

```c#
// GET api/Room/{i}
public async Task<string> GetIt(int i)
{
    await _applicationDbContext.SaveChangesAsync();
    return $"otrzymałem {i}";
}

// GET api/Room/{i}/{j}
[HttpGet("{i}/{j}")]
public string DoSth(int i, string j)
{
    return $"otrzymałem {i} oraz {j}";
}

// GET api/Room
public string GetIt()
{
    return $"otrzymałem coś :)";
}

// GET api/Room/topSecret
[HttpGet("topSecret")]
public IActionResult Forbid()
{
    return new UnauthorizedResult();
}

// POST  api/Room + body
public int Post(SomeClass x)
{
    if (x.I == 2)
    {
        return 4;
    }
    if (x.X == string.Empty)
    {
        return 56;
    }
    if (x.X == "aa")
    {
        return 36;
    }

    return 3;
}
```

## Baza danych
W projekcie zastosowano podejście CodeFirst z EntityFramework Core 3.1.
Do tworzenia i aplikowania migracji wykorzystywany jest projekt UnusedWebApp. Connection string konfigurowalny w pliku startup.

Dla lokalnego działania aplikacji, connection string należy umiescić w pliku local.settings.json, o następującej strukturze:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    },
    "ConnectionStrings": {
        "SqlConnectionString": "Server=(localdb)\\mssqllocaldb;Database=Test;ConnectRetryCount=0"
    },
    "Host": {
        "CORS": "*"
    }
}
```
Encje bazodanowe deklarowane są w projekcie Domain, a kontekst bazodanowy w Database.

## CI/CD
Skrypty budujące i deployujące znajdują się w folderze scripts.
Maszyna budująca znajduje się pod adresem \\VM-TAM-CI
Na maszynie budującej powinno znajdować się:
- msbuild tools 2019
- yarn
- git
- az cli
- Az and Azure powershell modules installed:
    - Install-Module -Name Az -AllowClobber -Scope AllUsers
    - Install-Module -Name Azure -AllowClobber -Scope AllUsers
- .net core 3.1

### Environment variables
Aplikacja kliencka podczas budowania używa zmiennych środowiskowych. Na serwerze CI nalezy je skonfigurować w sekcji:
Ustawienia -> CI/CD -> Variables

Podczas lokalnego budowania, trzeba je zadeklarować w Zmiennych środowiskowych:

![](/images/readmd/var.png)

Zmienne i wartości dla lokalnego budowania i testowania:
- REACT_APP_TAM_AUTHORITY - https://login.microsoftonline.com/futureprocessinguk.onmicrosoft.com
- REACT_APP_TAM_CLIENT_ID - zapytaj lokalnego azure devopsa ;)
- REACT_APP_TAM_FRONTEND_URL - http://localhost:3000
- REACT_APP_TAM_BACKEND_URL - http://localhost:7071
- REACT_APP_TAM_USER_IMPERSONATION_SCOPE - https://teamsallocationmanagerapi.azurewebsites.net/user_impersonation 

## Infrastracture
Aplikacja wykorzystuje Azure Functions App jako backend, frontend jest hostowany jako strona statyczna na Storage Account.
Dane przechowywane są w bazie AzureSql. Monitorowanie stanu aplikacji odbywa się przez AppInsights.

![](/images/readmd/infr.png)

## Auth
Logowanie w aplikacji oparte jest o Azure Active Directory.

## Frontend
Aplikacja stworzona za pomocą szablonu create-react-app.