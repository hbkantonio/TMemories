import React from "react"
import {
    Button,
    Form,
    FormGroup,
    Input,
    Label
} from 'reactstrap';
import { useForm } from "react-hook-form";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import Swal from 'sweetalert2'
import { accountService } from "../../services/account.service";

export function Login() {

    const schema = Yup.object().shape(
        {
            email: Yup.string()
                .required("Este campo es requerido")
                .email("No parece un email valido")
            ,
            password: Yup.string()
                .required("Este campo es requerido")
        });

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({ resolver: yupResolver(schema) });

    const onSubmit = (model) => {
        debugger;
        accountService.login(model).then(result => {
            if (!result.IsSuccessful)
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: result.message
                })
        });
    }

    return (
        <div >
            <h2>Iniciar sesión</h2>
            <Form className="form" onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="email">Email</Label>
                    <input
                        type="email"
                        className={`form-control ${errors.email && "is-invalid"}`}
                        {...register("email")}
                    />
                    <div className="invalid-feedback">{errors.email?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="password">Password</Label>
                    <input
                        type="password"
                        className={`form-control ${errors.password && "is-invalid"}`}
                        {...register("password")}
                    />
                    <div className="invalid-feedback">{errors.password?.message}</div>
                </FormGroup>
                <Button>Entrar</Button>
            </Form>
            <hr />
            <div style={{ textAlign: "center" }}>
                <a href={`${process.env.REACT_APP_API_URL}/account/ExternalLogin/Facebook`}>Iniciar sesión con Facebook</a>
                <hr />
                <a href={`${process.env.REACT_APP_API_URL}/account/ExternalLogin/Google`}>Iniciar sesión con Google</a>
                <hr />
                <a href="/forgot-password">¿Has olvidado la contraseña?</a>
                <hr />
                ¿No tienes una cuenta? <a href="/register">Crear cuenta</a>
            </div>
        </div>
    )
}