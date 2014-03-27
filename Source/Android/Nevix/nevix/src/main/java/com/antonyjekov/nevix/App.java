package com.antonyjekov.nevix;

import android.app.Application;
import android.content.Context;
import android.net.ConnectivityManager;

/**
 * Created by ajekov on 3/27/2014.
 */
public class App extends Application {

    public boolean isNetworkConnected() {
        return ((ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE)).getActiveNetworkInfo() != null;
    }
}
