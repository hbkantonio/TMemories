import React from "react"
import {
    Button,
    Form,
    FormGroup,
    Input,
    Label
} from 'reactstrap';
import { accountService } from "../../services/account.service";

export function ForgotPassword() {

    const onSubmit = () => {

    }

    return (
            <div>
                <h2>¿Tienes problemas para iniciar sesión?</h2>
                <Form className="form">
                    <FormGroup>
                        <Label for="email">Ingresa tu correo electrónico y te enviaremos un enlace para que recuperes el acceso a tu cuenta.</Label>
                        <Input
                            type="email"
                            name="email"
                            id="emial"
                            placeholder="example@example.com"
                        />
                    </FormGroup>
                    <Button>Enviar enlace</Button>
                    <br />
                    <a href="/login" >Volver al inicio de sesión</a>
                </Form>
            </div>
    )
}