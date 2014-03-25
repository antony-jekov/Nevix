package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Rect;

import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.buttons.FastForwardButton;
import com.antonyjekov.nevix.player.buttons.FullScreenButton;
import com.antonyjekov.nevix.player.buttons.NextButton;
import com.antonyjekov.nevix.player.buttons.PlayButton;
import com.antonyjekov.nevix.player.buttons.PreviousButton;
import com.antonyjekov.nevix.player.buttons.RewindButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeDownButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeUpButton;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class BSPlayer extends Player {

    public BSPlayer(Context context, PusherManager pusher) {
        super(context, pusher);
    }

    @Override
    protected void arrangeButtons() {
        int halfWidth = width >> 1;
        int halfHeight = height >> 1;

        int buttonHalfSize = ((int) (width * .15)) >> 1;
        Rect playBtn = new Rect(halfWidth - buttonHalfSize, halfHeight - buttonHalfSize, halfWidth + buttonHalfSize, halfHeight + buttonHalfSize);
        buttons.add(new PlayButton(playBtn));

        int buttonSize = ((int) (width * .12));

        Rect ffBtn = rightTo(playBtn, buttonSize);
        buttons.add(new FastForwardButton(ffBtn));

        Rect nextBtn = rightTo(ffBtn, buttonSize);
        buttons.add(new NextButton(nextBtn));

        Rect rwBtn = leftTo(playBtn, buttonSize);
        buttons.add(new RewindButton(rwBtn));

        Rect prevBtn = leftTo(rwBtn, buttonSize);
        buttons.add(new PreviousButton(prevBtn));

        buttonSize = (int) (width * .08);
        Rect fullBtn = new Rect(width - (buttonSize + buttonMargin), buttonMargin, width - buttonMargin, buttonMargin + buttonSize);
        buttons.add(new FullScreenButton(fullBtn));

        Rect volUpBtn = leftTo(fullBtn, buttonSize);
        buttons.add(new SystemVolumeUpButton(volUpBtn));

        Rect volumeDownBtn = leftTo(volUpBtn, buttonSize);
        buttons.add(new SystemVolumeDownButton(volumeDownBtn));
    }
}
