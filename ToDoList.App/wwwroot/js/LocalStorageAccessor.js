export function get(key) {
    return window.localStorage.getItem(key); // clau-valor, li passem la clau, llavors ens donen el valor, per la key tipus string
}
// una funció exportada, és visibilitat publica, altres moduls accedeixen amb metodes

export function set(key, value) {
    window.localStorage.setItem(key, value);
}

// netejar el que tinguem el storage
export function clear() {
    window.localStorage.clear();
}

// borrem l'item per key. 
export function remove(key) {
    window.localStorage.removeItem(key);
}
// accedir JS desde el CSharp