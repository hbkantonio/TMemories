import React, { useState } from 'react';
import { catalogService } from "../../services/catalog.service";

export function SelectCustom(props) {
    const [items, setItems] = useState([]);
    React.useEffect(() => {
        getCatalog();
    }, [props.method]);

    async function getCatalog() {
        catalogService.getAll(props.method).then(result => setItems(result.data));
    }

    return <select
        className={`form-control`}
        name={props.name}
        id={props.id}
        onChange={props.onChange}>
        <option>Selecciona una opción</option>
        {
            items.map(item =>
                <option key={item.value} value={item.value}>
                    {item.text}
                </option>
            )
        }
    </select>;
}
