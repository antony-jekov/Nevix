package com.antonyjekov.nevix.fragments;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.PersistentManager;

public class AuthorizationFragment extends Fragment {
    Button loginBtn;
    Button registerBtn;
    Button gotoLoginBtn;
    Button gotoRegisterBtn;

    EditText email;
    EditText pass;
    EditText confirm;

    PersistentManager persistent;
    Boolean isLogin;

    public AuthorizationFragment(PersistentManager persister) {
        this.persistent = persister;
        isLogin = true;
    }

    public static final String SESSION_KEY = "com.antonyjekov.nevix.login.sessionKey";

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        final View loginView = inflater.inflate(R.layout.fragment_authenticate, container, false);

        email = (EditText) loginView.findViewById(R.id.auth_email);
        pass = (EditText) loginView.findViewById(R.id.auth_pass);
        confirm = (EditText) loginView.findViewById(R.id.auth_confirm);

        loginBtn = (Button) loginView.findViewById(R.id.auth_login_btn);
        loginBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString();
                String passwordText = pass.getText().toString();

                if (emailText.length() > 0 && passwordText.length() > 0) {
                    persistent.login(emailText, passwordText);
                } else {
                    warnUser("Please fill in all fields!");
                }
            }
        });

        registerBtn = (Button) loginView.findViewById(R.id.auth_register_btn);
        registerBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString();
                String passwordText = pass.getText().toString();
                String confirmText = confirm.getText().toString();
                if (emailText.length() == 0 || passwordText.length() == 0 || confirmText.length() == 0) {
                    warnUser("Please fill in all fields!");
                }
                else if (passwordText.equals(confirmText)) {
                    persistent.register(emailText, passwordText, confirmText);
                } else {
                    warnUser("Passwords do not match!");
                }
            }
        });

        gotoLoginBtn = (Button) loginView.findViewById(R.id.auth_goto_login);

        View.OnClickListener toggleListener = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (isLogin) {
                    isLogin = false;
                    loginBtn.setVisibility(View.GONE);
                    confirm.setVisibility(View.VISIBLE);
                    registerBtn.setVisibility(View.VISIBLE);
                    gotoLoginBtn.setVisibility(View.VISIBLE);
                    gotoRegisterBtn.setVisibility(View.GONE);

                } else {
                    isLogin = true;
                    loginBtn.setVisibility(View.VISIBLE);
                    confirm.setVisibility(View.GONE);
                    registerBtn.setVisibility(View.GONE);
                    gotoLoginBtn.setVisibility(View.GONE);
                    gotoRegisterBtn.setVisibility(View.VISIBLE);
                }
            }
        };

        gotoLoginBtn.setOnClickListener(toggleListener);

        gotoRegisterBtn = (Button) loginView.findViewById(R.id.auth_goto_register);
        gotoRegisterBtn.setOnClickListener(toggleListener);

        return loginView;
    }

    private void warnUser(String message) {
        Toast.makeText(getActivity(), message, Toast.LENGTH_LONG).show();
    }
}
