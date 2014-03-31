package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/1/2014.
 */
public class MuteButton extends SphericalButton {
    public MuteButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        // TODO: draw icon
    }

    @Override
    public String command() {
        return PlayerCommand.MUTE_CMD;
    }
}
