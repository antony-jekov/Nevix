package com.antonyjekov.nevix.fragments;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.activities.BaseActivity;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersisterManager;
import com.antonyjekov.nevix.common.contracts.FailCallback;

public class AuthorizationFragment extends Fragment implements FailCallback {
    private Button loginBtn;
    private View resetBtn;
    private EditText email;
    private EditText pass;

    ProgressDialog progress;

    @Override
    public void onFail() {
        progress.hide();
        if (!checkInternetConnection()) {
            warnUser(getResources().getString(R.string.no_internet));
        } else {
            new AlertDialog.Builder(getActivity())
                    .setTitle(getResources().getString(R.string.login_fail))
                    .setMessage(getResources().getString(R.string.login_fail_message))
                    .setIcon(android.R.drawable.ic_dialog_alert)
                    .setPositiveButton(getResources().getString(R.string.yes), new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            new AlertDialog.Builder(getActivity())
                                    .setTitle(getResources().getString(R.string.provided_password))
                                    .setMessage("'" + pass.getText().toString() + "'")
                                    .show();
                        }
                    })
                    .setNegativeButton(getResources().getString(R.string.no), new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {

                        }
                    }).show();
        }
    }

    private boolean checkInternetConnection() {
        return ((BaseActivity) getActivity()).application().isNetworkConnected();
    }

    HttpAsyncRequest.OnResultCallBack callBack;

    public AuthorizationFragment(HttpAsyncRequest.OnResultCallBack callBack) {
        this.callBack = callBack;
    }

    public static final String SESSION_KEY = "com.antonyjekov.nevix.login.sessionKey";

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        final View loginView = inflater.inflate(R.layout.fragment_authenticate, container, false);

        progress = new ProgressDialog(getActivity());
        progress.setTitle(getResources().getString(R.string.logging_in));
        progress.setMessage(getResources().getString(R.string.connecting_wait));

        final PersisterManager persistent = new PersisterManager();
        persistent.wakeUpInternet();

        email = (EditText) loginView.findViewById(R.id.auth_email);
        pass = (EditText) loginView.findViewById(R.id.auth_pass);
        final FailCallback error = this;
        loginBtn = (Button) loginView.findViewById(R.id.auth_login_btn);
        loginBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString().trim();
                String passwordText = pass.getText().toString().trim();

                if (emailText.length() > 0 && passwordText.length() > 0) {
                    persistent.login(emailText, passwordText, callBack, error);
                    progress.show();
                } else {
                    warnUser(getResources().getString(R.string.fill_all_fields));
                }
            }
        });

        resetBtn = loginView.findViewById(R.id.auth_reset_pass);
        resetBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final String emailText = email.getText().toString().trim();

                if (emailText != null && emailText.length() > 0) {
                    new AlertDialog.Builder(getActivity())
                            .setTitle(getResources().getString(R.string.reset_pass_title))
                            .setMessage(getResources().getString(R.string.reset_pass_message))
                            .setIcon(android.R.drawable.ic_dialog_alert)
                            .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {

                                public void onClick(DialogInterface dialog, int whichButton) {
                                    persistent.resetPass(emailText, new HttpAsyncRequest.OnResultCallBack() {
                                                @Override
                                                public void onResult(String result) {
                                                    new AlertDialog.Builder(getActivity())
                                                            .setTitle(getResources().getString(R.string.reset_pass_completed_title))
                                                            .setMessage(getResources().getString(R.string.reset_pass_completed_message))
                                                            .setIcon(android.R.drawable.ic_dialog_info)
                                                            .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                                                                @Override
                                                                public void onClick(DialogInterface dialog, int which) {

                                                                }
                                                            })
                                                            .show();
                                                }
                                            },
                                            new FailCallback() {
                                                @Override
                                                public void onFail() {
                                                    warnUser(getResources().getString(R.string.reset_pass_failed_message));
                                                }
                                            }
                                    );
                                }
                            })
                            .setNegativeButton(android.R.string.no, null).show();
                } else {
                    warnUser(getResources().getString(R.string.provide_registration_email));
                }
            }
        });

        return loginView;
    }

    private void warnUser(String message) {
        Toast.makeText(getActivity(), message, Toast.LENGTH_LONG).show();
    }
}
