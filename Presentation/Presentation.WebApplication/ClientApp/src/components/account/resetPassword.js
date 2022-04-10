import React from "react"
import {
    Button,
    Form,
    FormGroup,
    Input,
    Label
} from 'reactstrap';
import { accountService } from "../../services/account.service";

export function ResetPassword() {

    const onSubmit = () => {

    }

    return (
        <div>
            <h2>Reestablecer contraseña</h2>
            <Form className="form">
                <FormGroup>
                    <Label for="password">Password</Label>
                    <Input
                        type="password"
                        name="password"
                        id="password"
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="confirmPassword">Confrimar password</Label>
                    <Input
                        type="password"
                        name="confirmPassword"
                        id="confirmPassword"
                    />
                </FormGroup>
                <Button>Reestablecer</Button>
            </Form>
            <br />
            <a href="/login" >Iniciar de sesión</a>
        </div>
    )
}