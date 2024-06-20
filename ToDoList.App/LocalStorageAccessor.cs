
using Microsoft.JSInterop;
using System.Text.Json;

namespace ToDoList.App {
    // fem Ctrl + ., per implementar el mètode de IAsyncDisposable
    public class LocalStorageAccessor : IAsyncDisposable
    {
        // GETTER, 
        public async Task<T> GetValueAsync<T>(string key) {
            await WaitForReference();
            var jsonResult = await _accessorJsRef.Value.InvokeAsync<string>("get", key);
            var result = JsonSerializer.Deserialize<T>(jsonResult);
            return result;
        }
        // tots els ue contenen await, són valors Async Task que retornar SETTER + a més li treiem <T> del Task
        public async Task SetValueAsync<T>(string key, T value) {
            await WaitForReference();
            string jsonValue = JsonSerializer.Serialize(value);
            await _accessorJsRef.Value.InvokeVoidAsync("set", key, jsonValue);
        }
        // remove, elimina segons la clau introduida
        public async Task RemoveValueAsync(string key) {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("remove", key);
        }
        // BUIDA EL LOCAL STORAGE
        public async Task ClearAsync() {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("clear");
        }
        // IJSObject reference, si pitgem tab
        private Lazy<IJSObjectReference> _accessorJsRef = new(); //emboliquem l'objecte amb lazy, que no sabem el valor fins que ho necessitem, només es genera quan l'usi o necessiti, no quan es crea la classe, o per primer construir la classe, el recurs encara no existeix pero necessitarem endevant
        // metode dispose asincro, allibera, gestio de memoria, no se sol usar un Dispose (quan alliberem l'instancia, quan l'executes, destructor.) ARA perque.NET no sap que te que lliberar
        private readonly IJSRuntime _jsRuntime; // propietat que nomes sinicialitza en el constructor, setejem el valor en el constructor
        
        // constructor: motor dexecucio de javascript IJSRuntime
        public LocalStorageAccessor(IJSRuntime jsRuntime) {
            _jsRuntime = jsRuntime;
        }
        
        // Asyncro: metode que sexecuta pero que no sabem quan retorna el valor. Normalment fem sequencialment per aixo sabem quan retorna, només una linea de execució. Asyncro, executa, en algun moment completara, mentres vaig fer execucions daltres lines dexecució
        private async Task WaitForReference() {
            // els async son molt facils de fer, i retorna una Task, que es una tasca que tindra el seu estat executar, per iniciar o completada, que dona un resultat, camp, ja sigui error o si ha funcionat, resultat assincro, si sexecuta, o completat, o be o no, o quin resultaT?
            // consulta si esta creat, si no esta creat, un nou await del invokeasync, "esperar que el tingui, per a que es crei i s'executi"
            if (!_accessorJsRef.IsValueCreated) {
                _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>
                ("import", "/js/LocalStorageAccessor.js"));
            }
        }
        
        public async ValueTask DisposeAsync()
        {
            if(_accessorJsRef.IsValueCreated) {
                // espera que completi un Asyncro.
                await _accessorJsRef.Value.DisposeAsync();
            }
        }
    }
}