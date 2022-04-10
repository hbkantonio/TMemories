import React from "react"
import { Link } from 'react-router-dom';
import {
    Button,
    Form,
    FormGroup,
    Label
} from 'reactstrap';
import { SelectCustom } from "../custom/selectCustom"
import { useForm } from "react-hook-form";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import Swal from 'sweetalert2'
import { accountService } from "../../services/account.service";
import moment from "moment";

export function Register() {

    const schema = Yup.object().shape(
        {
            firstName: Yup.string()
                .required("Este campo es requerido")
                .matches(/^[ a-zA-ZÀ-ÿ\u00f1\u00d1]*$/g, "Este campo solo admite letras"),
            lastName: Yup.string()
                .required("Este campo es requerido")
                .matches(/^[ a-zA-ZÀ-ÿ\u00f1\u00d1]*$/g, "Este campo solo admite letras"),
            email: Yup.string()
                .required("Este campo es requerido")
                .email("No parece un email valido"),
            password: Yup.string()
                .required("Este campo es requerido")
                .matches(
                    /^(?=.{8,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\W).*$/g,
                    "Debe tener al menos 8 caracteres, al menos un símbolo, al menos una mayúscula y minúscula"
                ),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password"), null], "Las contraseñas deben coincidir")
                .required("Este campo es requerido"),
            phoneNumber: Yup.string().when("phoneNumber", {
                is: (value) => value?.length,
                then: (rule) =>
                    rule.matches(/^[0-9]{10}$/g, "No parece un número de teléfono valido"),
            }),
            dateBirth: Yup.date()
                .typeError("Este campo es requerido")
                .min("1900-01-01", "El valor debe ser mayor o igual a 01/01/1900")
                .max(
                    moment(new Date()).format("YYYY-MM-DD"),
                    "El valor debe ser menor o igual a " +
                    moment(new Date()).format("DD/MM/YYYY")
                )
                .required("Este campo es requerido"),
            gender: Yup.string().nullable().required("Este campo es requerido"),
            country: Yup.string().nullable().required("Este campo es requerido")
        },
        [
            ["phoneNumber", "phoneNumber"]
        ]
    );

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({ resolver: yupResolver(schema) });

    const onSubmit = (model) => {
        debugger;
        accountService.register(model).then(result => {
            if (result.IsSuccessful)
                Swal.fire({
                    icon: 'error',
                    text: result.message
                })
            else {
                Swal.fire({
                    icon: 'success',
                    text: result.message
                }).then(()=> )
            }
        });
    }

    return (
        <div>
            <h2>Crear cuenta</h2>
            <Form className="form" onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="firstName">Nombre</Label>
                    <input
                        type="text"
                        className={`form-control ${errors.firstName && "is-invalid"}`}
                        {...register("firstName")}
                    />
                    <div className="invalid-feedback">{errors.firstName?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="lastName">Apellidos</Label>
                    <input
                        type="text"
                        className={`form-control ${errors.lastName && "is-invalid"}`}
                        {...register("lastName")}
                    />
                    <div className="invalid-feedback">{errors.lastName?.message}</div>
                </FormGroup>
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
                        {...register("email")}
                    />
                    <div className="invalid-feedback">{errors.password?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="confirmPassword">Confrimar password</Label>
                    <input
                        type="password"
                        className={`form-control ${errors.confirmPassword && "is-invalid"}`}
                        {...register("confirmPassword")}
                    />
                    <div className="invalid-feedback">{errors.confirmPassword?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="phoneNumber">Teléfono</Label>
                    <input
                        type="text"
                        className={`form-control ${errors.phoneNumber && "is-invalid"}`}
                        {...register("phoneNumber")}
                    />
                    <div className="invalid-feedback">{errors.phoneNumber?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="dateBirth">Fecha de nacimiento</Label>
                    <input
                        type="date"
                        className={`form-control ${errors.dateBirth && "is-invalid"}`}
                        {...register("dateBirth")}
                    />
                    <div className="invalid-feedback">{errors.dateBirth?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="gender">Genero</Label>
                    <select
                        className={`form-control ${errors.gender && "is-invalid"}`}
                        {...register("gender")}>
                        <option>Selecciona una opción</option>
                        <option>Hombre</option>
                        <option>Mujer</option>
                    </select>
                    <div className="invalid-feedback">{errors.gender?.message}</div>
                </FormGroup>
                <FormGroup>
                    <Label for="country">País de origen</Label>
                    <SelectCustom
                        method="countries"
                        {...register("country")}
                    />
                    <div className="invalid-feedback">{errors.country?.message}</div>
                </FormGroup>
                <Button>Crear</Button>
            </Form>
            <br />
            <a href="/login" >Volver al inicio de sesión</a>
        </div>
    )
}