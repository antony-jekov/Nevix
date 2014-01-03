package com.antonyjekov.nevix.models;

public class UserRegisterViewModel extends UserLoginModel {
    private String confirmPassword;

    public UserRegisterViewModel(String email, String pass, String confirmPass){
        super(email, pass);

        this.confirmPassword = confirmPass;
    }

    public String getConfirmPassword() {
        return confirmPassword;
    }

    public void setConfirmPassword(String confirmPassword) {
        this.confirmPassword = confirmPassword;
    }
}
