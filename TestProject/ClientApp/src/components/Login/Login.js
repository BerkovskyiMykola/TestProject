import React, { useState } from "react";
import { useTranslation } from "react-i18next";

import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { Redirect } from "react-router-dom";
import { login } from "../../actions/auth";
import { validateEmail } from "../../validation/validation"

import { Form } from "../FormComponents";

import {
    Button,
    FormFeedback,
    FormGroup,
    Input,
    Label
} from 'reactstrap';
import handleChange from "../../utils/handleChange";

export default function Login() {
    const { t } = useTranslation();
    const [model, setModel] = useState({ email: "", password: "" });
    const [validate, setValidate] = useState({ email: '' });

    const dispatch = useDispatch();

    const { message, isLoggedIn } = useSelector(state => ({
        message: state.message.message,
        isLoggedIn: state.auth.isLoggedIn
    }), shallowEqual)

    const submitForm = (e) => {
        e.preventDefault();

        dispatch(login(model.email, model.password))
            .then(() => { })
            .catch(() => { });
    }

    if (isLoggedIn) {
        return <Redirect to="/profile" />;
    }

    return (
        <div className="col-md-12">
            <div className="card card-container">
                <Form handleSubmit={submitForm} message={message}>
                    <FormGroup>
                        <Label for="email">{t("email")}</Label>
                        <Input
                            type="email"
                            name="email"
                            id="email"
                            placeholder="example@example.com"
                            required
                            valid={validate.email === "has-success"}
                            invalid={validate.email === "has-danger"}
                            value={model.email}
                            onChange={(e) => {
                                validateEmail(validate, setValidate)(e);
                                handleChange(model, setModel)(e);
                            }}
                        />
                        <FormFeedback>
                            Uh oh! Looks like there is an issue with your email. Please input
                            a correct email.
                        </FormFeedback>
                        <FormFeedback valid>
                            That's a tasty looking email you've got there.
                        </FormFeedback>
                    </FormGroup>
                    <FormGroup>
                        <Label for="password">{t("password")}</Label>
                        <Input
                            type="password"
                            name="password"
                            id="password"
                            placeholder="********"
                            value={model.password}
                            onChange={handleChange(model, setModel)}
                            minLength={8}
                            maxLength={18}
                        />
                    </FormGroup>
                    <FormGroup>
                        <Button block color="primary">{t("Login")}</Button>
                    </FormGroup>
                </Form>
            </div>
        </div>
    )
}