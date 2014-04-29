package com.antonyjekov.nevix.activities;

import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;

import com.antonyjekov.nevix.App;

/**
 * Created by ajekov on 3/27/2014.
 */
public abstract class BaseActivity extends ActionBarActivity {

    protected App application;

    protected BaseActivity() {
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        this.application = (App) getApplication();
    }

    public App application() {
        return this.application;
    }
}
