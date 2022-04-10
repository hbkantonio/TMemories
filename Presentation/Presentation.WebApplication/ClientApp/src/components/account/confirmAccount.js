import React from "react"
import {
    Alert
} from 'reactstrap';
import { accountService } from "../../services/account.service";

export function ConfirmAccount() {

    const onSubmit = () => {

    }

    return (
        <div>
            <h2>Confirmación de cuenta</h2>
            <Alert color="success">
                Tu cuenta se confirmó correctamente
            </Alert>
            <Alert color="danger">
                Ocurrió un error al confirmar tu cuenta
            </Alert>
            <br />
            <a href="/login" >Iniciar de sesión</a>
        </div>
    )
}