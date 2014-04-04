package com.antonyjekov.nevix.common;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.antonyjekov.nevix.R;

/**
 * Created by Antony Jekov on 4/3/2014.
 */
public class FolderAdapter extends ArrayAdapter {

    private int layoutResourceId;
    private Context context;
    private Object[] mediaItems;

    public FolderAdapter(Context context, int resource, Object[] objects) {
        super(context, resource, objects);

        this.layoutResourceId = resource;
        this.context = context;
        this.mediaItems = objects;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater inflater = ((Activity) context).getLayoutInflater();

        LinearLayout row = (LinearLayout) inflater.inflate(R.layout.media_item, null, false);

        ImageView icon = (ImageView) row.findViewById(R.id.media_item_icon);
        TextView title = (TextView) row.findViewById(R.id.media_item_title);


        MediaItem item = (MediaItem) this.mediaItems[position];
        title.setText(item.title);
        icon.setImageResource(item.iconResourceId);

        return row;
    }

    static class MediaHolder {
        ImageView icon;
        TextView title;
    }
}