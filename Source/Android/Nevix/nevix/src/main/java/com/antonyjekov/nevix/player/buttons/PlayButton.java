package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class PlayButton extends SphericalButton {
    public PlayButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);
        int width = button.width() - ((strokeWidth << 1) + (padding << 1));
        int halfWidth = width >> 1;
        float startingX = button.centerX() - (halfWidth >> 1);
        float startingY = button.centerY() - halfWidth;

        shape.moveTo(startingX, startingY);
        shape.lineTo(startingX, startingY + width);
        shape.lineTo(startingX + (width - (halfWidth >> 1)), button.centerY());
        shape.lineTo(startingX, startingY);
    }

    @Override
    public String command() {
        return PlayerCommand.PLAY_CMD;
    }
}
